using AwaraIT.Training.Plugins.PluginExtensions.Attributes;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using System;

namespace AwaraIT.Training.Plugins.PluginExtensions.Builder
{
    public class ContextWrapper : IContextWrapper
    {
        public const string DefaultImageKey = "image";
        private readonly IServiceProvider _serviceProvider;
        private object _targetObj;
        private object _retrivedObj;

        public Entity TargetEntity
        {
            get
            {
                if (_targetObj == null)
                    throw new InvalidOperationException("Целевая сущность недоступна на данном этапе.");
                return _targetObj as Entity;
            }
        }
        public Entity RetrivedEntity
        {
            get
            {
                if (_retrivedObj == null)
                    throw new InvalidOperationException("Получение сущности недоступно на данном этапе");
                return (Entity)_retrivedObj;
            }
            set
            {
                _retrivedObj = value;
            }
        }

        public EntityReference TargetEntityReference
        {
            get
            {
                if (_targetObj == null)
                    throw new InvalidOperationException("Целевая сущность недоступна на данном этапе.");
                return _targetObj as EntityReference;
            }
        }

        public EntityReferenceCollection RelatedEntities
        {
            get
            {
                if (Context.InputParameters.ContainsKey(nameof(RelatedEntities)))
                    return Context.InputParameters[nameof(RelatedEntities)] as EntityReferenceCollection;
                return (EntityReferenceCollection)null;
            }
        }

        public string EntityLogicalName
        {
            get
            {
                if (_targetObj is EntityReference)
                    return TargetEntityReference.LogicalName;
                if (_targetObj is Entity)
                    return TargetEntity.LogicalName;
                return "";
            }
        }

        public Entity PreImage { get; private set; }

        public Entity PostImage { get; private set; }

        public Entity Image
        {
            get
            {
                return PreImage ?? PostImage;
            }
        }

        public string Operation { get; private set; }

        public PluginStage Stage { get; private set; }

        public IOrganizationService Service { get; private set; }
        public ITracingService TracingService { get; private set; }

        public IPluginExecutionContext Context { get; private set; }

        public IOrganizationService GetOrganizationService(Guid? userId)
        {
            return _serviceProvider.Get<IOrganizationServiceFactory>().CreateOrganizationService(userId);
        }

        public ContextWrapper(IServiceProvider serviceProvider)
        {
            Argument.NotNull((object)serviceProvider, "ServiceProvider is required.");
            _serviceProvider = serviceProvider;
            Context = serviceProvider.Get<IPluginExecutionContext>();
            Service = serviceProvider.Get<IOrganizationServiceFactory>().CreateOrganizationService(new Guid?(Context.UserId));
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            if (Context.PreEntityImages.ContainsKey("image"))
                PreImage = Context.PreEntityImages["image"];
            if (Context.PostEntityImages.ContainsKey("image"))
                PostImage = Context.PostEntityImages["image"];
            Operation = Context.MessageName;
            Stage = (PluginStage)Context.Stage;
            if (Context.InputParameters.ContainsKey("Target"))
            {
                _targetObj = Context.InputParameters["Target"];
            }
            else
            {
                if (!Context.InputParameters.ContainsKey("EntityMoniker"))
                    return;
                _targetObj = Context.InputParameters["EntityMoniker"];
            }
        }
    }
}
