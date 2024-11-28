using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System.ComponentModel;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class Product : BaseEntity
    {
        public Product() : base(EntityLogicalName) { }
        public const string EntityLogicalName = "arl_products";

        public static class Metadata
        {
            public const string EduFormat = "arl_educformatid";
            public const string Subject = "arl_subjectid";
            public const string PrepFormat = "arl_prepformatid";
        }

        public EntityReference EduFormat
        {
            get { return GetAttributeValue<EntityReference>(Metadata.EduFormat); }
            set { Attributes[Metadata.EduFormat] = value; }
        }
        public EntityReference Subject
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Subject); }
            set { Attributes[Metadata.Subject] = value; }
        }
        public EntityReference PrepFormat
        {
            get { return GetAttributeValue<EntityReference>(Metadata.PrepFormat); }
            set { Attributes[Metadata.PrepFormat] = value; }
        }
    }
}
