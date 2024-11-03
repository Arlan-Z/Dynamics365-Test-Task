using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Plugins.PluginExtensions.Attributes;
using AwaraIT.Training.Plugins.PluginExtensions.Builder;
using AwaraIT.Training.Plugins.PluginExtensions.Extensions;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AwaraIT.Training.Plugins.PluginExtensions
{
    public abstract class PluginBase : IPlugin
    {
        private readonly List<PluginSubscription> _subscriptions = new List<PluginSubscription>();
        private readonly List<ExceptionHandlerSubscription> _exceptionHandlers = new List<ExceptionHandlerSubscription>();
        private Dictionary<string, string> _config = new Dictionary<string, string>();
        private ITracingService _tracingService;
        private bool _configLoadError;

        protected IPluginSubscriptionBuilder Subscribe
        {
            get
            {
                return new PluginSubscriptionBuilder(_subscriptions);
            }
        }

        protected void HandleException<TException>(Func<TException, bool> handler) where TException : Exception
        {
            _exceptionHandlers.Add(ExceptionHandlerSubscription.Create<TException>(handler));
        }

        protected PluginBase()
        {
        }

        protected PluginBase(string unsecureConfig, string secureConfig)
        {
            if (string.IsNullOrEmpty(unsecureConfig))
                return;
            try
            {
                _config = JsonSerializer.Deserialize<Dictionary<string, string>>(unsecureConfig);
            }
            catch
            {
                _configLoadError = true;
            }
        }

        protected TValue GetConfigParameter<TValue>(string parameterKey, TValue defaultValue)
        {
            if (!_configLoadError)
                return _config.GetValueOrDefault<TValue>(parameterKey, defaultValue);
            Trace(string.Format("Configuration key [{0}] requested, but plugin unsecure config is not a valid JSON.", (object)parameterKey));
            return defaultValue;
        }

        protected void Trace(string message, params object[] args)
        {
            _tracingService.Trace(message, args);
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            Argument.NotNull(serviceProvider, "Service provider is required.");
            if (!_subscriptions.Any())
                throw new InvalidPluginExecutionException("Ошибка регистрации плагина. Обратитесь к разработчику (No subscriptions).");
            _tracingService = serviceProvider.Get<ITracingService>();
            ContextWrapper wrapper = new ContextWrapper(serviceProvider);

            foreach (PluginSubscription subscription in _subscriptions)
            {
                if (subscription.IsSuitable(wrapper))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    try
                    {
                        OnBeforeHandlerExecuted(wrapper);
                        if (subscription.RetriveFields.Any() && (wrapper.TargetEntity != null || wrapper.TargetEntityReference != null))
                        {
                            if (wrapper.TargetEntity != null)
                            {
                                wrapper.RetrivedEntity = wrapper.Service.Retrieve(wrapper.EntityLogicalName,
                                    wrapper.TargetEntity.Id,
                                    new ColumnSet(subscription.RetriveFields.ToArray()));
                            }
                            else if (wrapper.TargetEntityReference != null)
                            {
                                wrapper.RetrivedEntity = wrapper.Service.Retrieve(wrapper.EntityLogicalName,
                                    wrapper.TargetEntityReference.Id,
                                    new ColumnSet(subscription.RetriveFields.ToArray()));
                            }
                        }

                        subscription.Handler(wrapper);
                        OnAfterHandlerExecuted(wrapper);
                        stopwatch.Stop();
                        //Trace(string.Format("Plugin {0} stage {1} entity {2} finished in {3}", GetType().FullName, wrapper.Stage, wrapper.EntityLogicalName, stopwatch.Elapsed));
                    }
                    catch (Exception ex)
                    {
                        Trace("Ошибка работы плагина:\r\n{0}", (object)ex.ToString());
                        foreach (ExceptionHandlerSubscription exceptionHandler in _exceptionHandlers)
                        {
                            if (exceptionHandler.IsSuitble(ex) && exceptionHandler.Handle(ex))
                                return;
                        }
                        throw;
                    }
                }
            }
        }

        protected virtual void OnBeforeHandlerExecuted(IContextWrapper wrapper)
        {

        }

        protected virtual void OnAfterHandlerExecuted(IContextWrapper wrapper)
        {
        }
    }
}
