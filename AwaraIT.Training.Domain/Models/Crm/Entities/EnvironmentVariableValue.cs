using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class EnvironmentVariableValue : BaseEntity
    {
        public EnvironmentVariableValue() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string EnvironmentVariableDefinitionId = "environmentvariabledefinitionid";
            public const string Value = "value";
        }

        public const string EntityLogicalName = "environmentvariablevalue";

        public EntityReference EnvironmentVariableDefinitionId
        {
            get { return GetAttributeValue<EntityReference>(Metadata.EnvironmentVariableDefinitionId); }
            set { Attributes[Metadata.EnvironmentVariableDefinitionId] = value; }
        }

        public string Value
        {
            get { return GetAttributeValue<string>(Metadata.Value); }
            set { Attributes[Metadata.Value] = value; }
        }
    }
}
