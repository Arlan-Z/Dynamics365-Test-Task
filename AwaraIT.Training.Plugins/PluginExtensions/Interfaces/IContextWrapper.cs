using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using Microsoft.Xrm.Sdk;
using System;

namespace AwaraIT.Training.Plugins.PluginExtensions.Interfaces
{
    public interface IContextWrapper
    {
        Entity TargetEntity { get; }
        Entity RetrivedEntity { get; set; }

        EntityReference TargetEntityReference { get; }

        EntityReferenceCollection RelatedEntities { get; }

        string EntityLogicalName { get; }

        Entity PreImage { get; }

        Entity PostImage { get; }

        Entity Image { get; }

        string Operation { get; }

        PluginStage Stage { get; }

        IOrganizationService Service { get; }

        ITracingService TracingService { get; }

        IPluginExecutionContext Context { get; }

        IOrganizationService GetOrganizationService(Guid? userId);

    }
}
