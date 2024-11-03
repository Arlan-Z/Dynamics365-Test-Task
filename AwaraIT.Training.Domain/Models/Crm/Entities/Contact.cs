using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class Contact : BaseEntity
    {
        public Contact() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string FullName = "fullname";
            public const string FirstName = "firstname";
            public const string LastName = "lastname";
            public const string MobilePhone = "mobilephone";
        }

        public const string EntityLogicalName = "contact";

        public string FullName
        {
            get { return GetAttributeValue<string>(Metadata.FullName); }
            set { Attributes[Metadata.FullName] = value; }
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

        public string MobilePhone
        {
            get { return GetAttributeValue<string>(Metadata.MobilePhone); }
            set { Attributes[Metadata.MobilePhone] = value; }
        }
    }
}
