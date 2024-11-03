using AwaraIT.Training.Plugins.PluginExtensions;
using System;
using System.Activities;

namespace AwaraIT.Training.Plugins.Contacts
{
    public class TestAction : BasicActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            base.Execute(executionContext);

            try
            {
                Logger.INFO("Action TestAction", $"Ok: {DateTime.UtcNow}");
            }
            catch (Exception ex)
            {
                Logger.ERROR("Action TestAction", ex.ToString());
            }
        }
    }
}
