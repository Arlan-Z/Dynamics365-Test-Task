using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class EnvironmentVariableDefinition : BaseEntity
    {
        public EnvironmentVariableDefinition() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string EnvironmentVariableDefinitionId = "environmentvariabledefinitionid";
            public const string DefaultValue = "defaultvalue";
            public const string SchemaName = "schemaname";
        }

        public const string EntityLogicalName = "environmentvariabledefinition";

        public string DefaultValue
        {
            get { return GetAttributeValue<string>(Metadata.DefaultValue); }
            set { Attributes[Metadata.DefaultValue] = value; }
        }

        public string SchemaName
        {
            get { return GetAttributeValue<string>(Metadata.SchemaName); }
            set { Attributes[Metadata.SchemaName] = value; }
        }
    }
}
