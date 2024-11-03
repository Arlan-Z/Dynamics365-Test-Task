using AwaraIT.Training.Plugins.PluginExtensions.Attributes;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Extensions;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwaraIT.Training.Plugins.PluginExtensions.Builder
{
    public class PluginSubscription
    {
        public string Message { get; private set; }

        public List<string> EntityNames { get; private set; }

        public List<PluginStage> Stages { get; private set; }

        public List<string> MandatoryUpdateFields { get; private set; }

        public List<string> EitherUpdateFields { get; private set; }
        public List<string> RetriveFields { get; private set; }

        public bool PreImageRequired { get; set; }

        public bool PostImageRequired { get; set; }

        public List<string> PreImageRequiredFields { get; private set; }

        public List<string> PostImageRequiredFields { get; private set; }

        public string AssociationName { get; set; }

        public bool AvoidRecursion { get; set; }

        public Action<IContextWrapper> Handler { get; private set; }

        public bool IsSuitable(IContextWrapper wrapper)
        {
            Argument.NotNull(wrapper, "Wrapper is required.");
            if ((!Message.EqualsIgnoreCase(wrapper.Operation)
                 || Stages.Count != 0 && !Stages.Contains(wrapper.Stage) ? 0 : (!EntityNames.Any() ? 1 : (EntityNames.Any(name => wrapper.EntityLogicalName.EqualsIgnoreCase(name)) ? 1 : 0))) == 0
                 || AvoidRecursion && wrapper.Context.Depth > 1
                 ||
                  (MandatoryUpdateFields.Any()
                 && !MandatoryUpdateFields.All(f => wrapper.TargetEntity.Attributes.ContainsKey(f))
                 || EitherUpdateFields.Any() && !EitherUpdateFields.Any(f => wrapper.TargetEntity.Attributes.ContainsKey(f))))
                return false;
            if (PreImageRequired && wrapper.PreImage == null)
                throw new InvalidPluginExecutionException("Ошибка регистрации плагина. Обратитесь к разработчику (PreImage not defined).");
            if (PostImageRequired && wrapper.PostImage == null)
                throw new InvalidPluginExecutionException("Ошибка регистрации плагина. Обратитесь к разработчику (PostImage not defined).");
            if (Message.ContainsIgnoreCase("associate") && !string.IsNullOrEmpty(AssociationName))
            {
                if (!wrapper.Context.InputParameters["Relationship"].ToString().Trim('.').EqualsIgnoreCase(AssociationName))
                    return false;
            }
            return true;
        }

        public void SetHandlder(Action<IContextWrapper> handler)
        {
            Argument.NotNull((object)handler, "Handler is required.");
            Handler = handler;
        }

        public PluginSubscription(string message)
        {
            Argument.NotNullOrEmpty(message, "Operation is required.");
            Message = message;
            Stages = new List<PluginStage>();
            EntityNames = new List<string>();
            MandatoryUpdateFields = new List<string>();
            EitherUpdateFields = new List<string>();
            PreImageRequiredFields = new List<string>();
            PostImageRequiredFields = new List<string>();
            RetriveFields = new List<string>();
        }
    }
}
