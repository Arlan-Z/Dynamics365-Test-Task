using AwaraIT.Training.Plugins.PluginExtensions.Attributes;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Extensions;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwaraIT.Training.Plugins.PluginExtensions.Builder
{
    public class PluginSubscriptionBuilder : IPluginSubscriptionBuilder, IPluginSubscribeToEntity, IPluginSubscribeToMessage
    {
        private readonly List<PluginSubscription> _allSubscriptions;
        private PluginSubscription _subscription;

        public IPluginSubscribeToEntity When(params PluginStage[] stages)
        {
            Argument.NotNull(stages, "Plugin stages are required.");
            Argument.Require(stages.Any(), "At least any plugin stage is required.");
            if (stages != null && stages.Any())
                _subscription.Stages.AddRange(stages);
            return this;
        }

        public void Execute(Action<IContextWrapper> handler)
        {
            Argument.NotNull(handler, "Message handler is required.");
            _subscription.SetHandlder(handler);
            _allSubscriptions.Add(_subscription);
        }

        public IPluginSubscribeToMessage ToMessage(string message)
        {
            Argument.NotNullOrEmpty(message, "Plugin message is required.");
            _subscription = new PluginSubscription(message);
            return (IPluginSubscribeToMessage)this;
        }

        public IPluginSubscribeToEntity PreImageRequired()
        {
            _subscription.PreImageRequired = true;
            return this;
        }

        public IPluginSubscribeToEntity PreImageRequired(params string[] fields)
        {
            if (fields == null || fields.Any(string.IsNullOrEmpty))
                throw new ArgumentException("PreImage required field names are required and should not be empty.");
            _subscription.PreImageRequired = true;
            _subscription.PreImageRequiredFields.AddRange(fields);
            return this;
        }

        public IPluginSubscribeToEntity PostImageRequired()
        {
            _subscription.PostImageRequired = true;
            return this;
        }

        public IPluginSubscribeToEntity PostImageRequired(params string[] fields)
        {
            if (fields == null || fields.Any((string.IsNullOrEmpty)))
                throw new ArgumentException("PostImage required field names are required and should not be empty.");
            _subscription.PostImageRequired = true;
            _subscription.PostImageRequiredFields.AddRange(fields);
            return this;
        }

        public IPluginSubscribeToEntity AvoidRecursion()
        {
            _subscription.AvoidRecursion = true;
            return this;
        }

        public IPluginSubscribeToEntity ThenRetriveFields(params string[] fields)
        {
            Argument.NotNull(fields, "Entity fields are required.");
            Argument.Require(fields.Any(), "At least one field should be specified.");
            if (_subscription.Message.EqualsIgnoreCase("create"))
                throw new InvalidOperationException("This method is prohibited for create.");
            _subscription.RetriveFields.AddRange(fields);
            return this;
        }

        public IPluginSubscribeToEntity WithAssociationName(
          string associationName)
        {
            Argument.NotNullOrEmpty(associationName, "Association name is required.");
            _subscription.AssociationName = associationName;
            return this;
        }

        public IPluginSubscribeToEntity ForEntity(string entityLogicalName)
        {
            Argument.NotNullOrEmpty(entityLogicalName, "Entity logical name is required.");
            _subscription.EntityNames.Add(entityLogicalName);
            return this;
        }

        public IPluginSubscribeToEntity ForEntities(
          params string[] entityLogicalNames)
        {
            if (entityLogicalNames == null || (entityLogicalNames).Any((string.IsNullOrEmpty)))
                throw new ArgumentException("Entity logical names are required and should not be empty.");
            _subscription.EntityNames.AddRange(entityLogicalNames);
            return this;
        }

        public IPluginSubscribeToEntity ForAnyEntity()
        {
            _subscription.EntityNames.Clear();
            return this;
        }

        public IPluginSubscribeToEntity WithAllFields(params string[] fields)
        {
            Argument.NotNull(fields, "Entity fields are required.");
            Argument.Require(fields.Any(), "At least any field should be specified.");
            if (!_subscription.Message.EqualsIgnoreCase("update"))
                throw new InvalidOperationException("This method is for Update messages only.");
            _subscription.MandatoryUpdateFields.AddRange((IEnumerable<string>)fields);
            return this;
        }

        public IPluginSubscribeToEntity WithAnyField(params string[] fields)
        {
            Argument.NotNull(fields, "Entity fields are required.");
            Argument.Require(fields.Any(), "At least any field should be specified.");
            if (!_subscription.Message.EqualsIgnoreCase("update"))
                throw new InvalidOperationException("This method is for Update messages only.");
            _subscription.EitherUpdateFields.AddRange(fields);
            return this;
        }

        public PluginSubscriptionBuilder(List<PluginSubscription> allSubscriptions)
        {
            Argument.NotNull(allSubscriptions, "Subscriptions list is required.");
            _allSubscriptions = allSubscriptions;
        }
    }
}
