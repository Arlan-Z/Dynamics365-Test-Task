using AwaraIT.Training.Plugins.PluginExtensions;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace AwaraIT.Training.Plugins.Contacts
{
    public class TestAction : BasicActivity
    {
        [Input("InputTest")]
        public InArgument<string> InputTest { get; set; }

        [Output("ErrorMessage")]
        public OutArgument<string> ErrorMessage { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            base.Execute(executionContext);

            var inputTest = InputTest.Get(executionContext);
            ErrorMessage.Set(executionContext, "");

            try
            {
                Logger.INFO("Action TestAction", $"Ok: {DateTime.UtcNow}. {inputTest}");
            }
            catch (Exception ex)
            {
                Logger.ERROR("Action TestAction", ex.ToString());
                ErrorMessage.Set(executionContext, ex.Message);
            }
        }
    }
}
