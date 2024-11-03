using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using System;

namespace AwaraIT.Training.Plugins.PluginExtensions.Interfaces
{
    public interface IPluginSubscribeToEntity
    {
        IPluginSubscribeToEntity When(params PluginStage[] stages);

        IPluginSubscribeToEntity PreImageRequired();

        IPluginSubscribeToEntity PreImageRequired(params string[] fields);

        IPluginSubscribeToEntity PostImageRequired();

        IPluginSubscribeToEntity PostImageRequired(params string[] fields);

        IPluginSubscribeToEntity AvoidRecursion();

        IPluginSubscribeToEntity WithAllFields(params string[] fields);

        IPluginSubscribeToEntity WithAnyField(params string[] fields);
        IPluginSubscribeToEntity ThenRetriveFields(params string[] fields);

        IPluginSubscribeToEntity WithAssociationName(string associationName);

        void Execute(Action<IContextWrapper> handler);
    }
}
