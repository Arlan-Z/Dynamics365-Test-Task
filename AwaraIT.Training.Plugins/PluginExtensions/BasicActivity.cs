using AwaraIT.Training.Application.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace AwaraIT.Training.Plugins.PluginExtensions
{
    public abstract class BasicActivity : CodeActivity
    {
        public IOrganizationService CrmService { get; set; }
        internal IWorkflowContext WorkflowContext { get; set; }

        public Logger Logger { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            CrmService = service;
            WorkflowContext = context;
            try
            {
                Logger = new Logger(service);
            }
            catch { }
        }
    }
}
