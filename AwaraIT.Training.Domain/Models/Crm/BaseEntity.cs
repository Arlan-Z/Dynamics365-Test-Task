using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.Serialization;

namespace AwaraIT.Training.Domain.Models.Crm
{
    [DataContract]
    public class BaseEntity : Entity
    {
        public BaseEntity(string crmEntityName) : base(crmEntityName) { }

        public DateTime CreatedOn
        {
            get { return GetAttributeValue<DateTime>(EntityCommon.CreatedOn); }
            set { Attributes[EntityCommon.CreatedOn] = value; }
        }

        public EntityReference CreatedBy => GetAttributeValue<EntityReference>(EntityCommon.CreatedBy);

        public DateTime ModifiedOn
        {
            get { return GetAttributeValue<DateTime>(EntityCommon.ModifiedOn); }
            set { Attributes[EntityCommon.ModifiedOn] = value; }
        }

        public EntityReference ModifiedBy => GetAttributeValue<EntityReference>(EntityCommon.ModifiedBy);

        public EntityReference OwnerId
        {
            get { return GetAttributeValue<EntityReference>(EntityCommon.OwnerId); }
            set { Attributes[EntityCommon.OwnerId] = value; }
        }
    }
}
