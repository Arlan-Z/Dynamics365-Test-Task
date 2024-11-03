using AwaraIT.Training.Domain.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class Log : BaseEntity
    {
        public Log() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string Level = "awr_level";
            public const string Description = "awr_description";
            public const string Subject = "awr_subject";
            public const string EntityType = "awr_entitytype";
            public const string EntityId = "awr_entityid";

            public enum LevelOptions
            {
                TRACE = 752_440_000,
                DEBUG = 752_440_001,
                INFO = 752_440_002,
                WARNING = 752_440_003,
                ERROR = 752_440_004,
                CRITICAL = 752_440_005
            }
        }

        public const string EntityLogicalName = "awr_log";

        public Metadata.LevelOptions? Level
        {
            get { return (Metadata.LevelOptions?)GetAttributeValue<OptionSetValue>(Metadata.Level)?.Value; }
            set { Attributes[Metadata.Level] = value != null ? new OptionSetValue((int)value.Value) : null; }
        }

        public string Description
        {
            get { return GetAttributeValue<string>(Metadata.Description); }
            set { Attributes[Metadata.Description] = value?.Crop(10000); }
        }

        public string Subject
        {
            get { return GetAttributeValue<string>(Metadata.Subject); }
            set { Attributes[Metadata.Subject] = value?.Crop(1000); }
        }

        public string EntityType
        {
            get { return GetAttributeValue<string>(Metadata.EntityType); }
            set { Attributes[Metadata.EntityType] = value; }
        }

        public string EntityId
        {
            get { return GetAttributeValue<string>(Metadata.EntityId); }
            set { Attributes[Metadata.EntityId] = value; }
        }
    }
}
