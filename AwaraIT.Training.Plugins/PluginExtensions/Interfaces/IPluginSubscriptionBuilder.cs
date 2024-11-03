namespace AwaraIT.Training.Plugins.PluginExtensions.Interfaces
{
    public interface IPluginSubscriptionBuilder
    {
        IPluginSubscribeToMessage ToMessage(string message);
    }
}
