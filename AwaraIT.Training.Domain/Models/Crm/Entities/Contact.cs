using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class Contact : BaseEntity
    {
        public Contact() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string FirstName = "arl_name";
            public const string MiddleName = "arl_middlename";
            public const string LastName = "arl_lastname";
            public const string PhoneNumber = "arl_phonenumber";
            public const string Email = "arl_email";
            public const string RegionId = "arl_regionid";
        }

        public const string EntityLogicalName = "arl_contact";

        public string MiddleName
        {
            get { return GetAttributeValue<string>(Metadata.MiddleName); }
            set { Attributes[Metadata.MiddleName] = value; }
        }

        public string FirstName
        {
            get { return GetAttributeValue<string>(Metadata.FirstName); }
            set { Attributes[Metadata.FirstName] = value; }
        }

        public string LastName
        {
            get { return GetAttributeValue<string>(Metadata.LastName); }
            set { Attributes[Metadata.LastName] = value; }
        }

        public string PhoneNumber
        {
            get { return GetAttributeValue<string>(Metadata.PhoneNumber); }
            set { Attributes[Metadata.PhoneNumber] = value; }
        }

        public string Email
        {
            get { return GetAttributeValue<string>(Metadata.Email); }
            set { Attributes[Metadata.Email] = value; }
        }

        public EntityReference RegionId
        {
            get { return GetAttributeValue<EntityReference>(Metadata.RegionId); }
            set { Attributes[Metadata.RegionId] = value; }
        }
    }
}
