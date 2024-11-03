using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.Serialization;

namespace AwaraIT.Training.Domain.Models.Crm
{
    [DataContract]
    public class BaseActionEntity : BaseEntity
    {
        public BaseActionEntity(string crmEntityName)
            : base(crmEntityName) { }

        public Guid ActivityId
        {
            get { return GetAttributeValue<Guid>(ActionCommon.ActivityId); }
            set { Attributes[ActionCommon.ActivityId] = value; }
        }

        public Entity[] From
        {
            get { return GetAttributeValue<Entity[]>(ActionCommon.From); }
            set { Attributes[ActionCommon.From] = value; }
        }

        public Entity[] To
        {
            get { return GetAttributeValue<Entity[]>(ActionCommon.To); }
            set { Attributes[ActionCommon.To] = value; }
        }

        public Entity[] Cс
        {
            get { return GetAttributeValue<Entity[]>(ActionCommon.Cc); }
            set { Attributes[ActionCommon.Cc] = value; }
        }

        public Entity[] Bcc
        {
            get { return GetAttributeValue<Entity[]>(ActionCommon.Bcc); }
            set { Attributes[ActionCommon.Bcc] = value; }
        }

        public string Subject
        {
            get { return GetAttributeValue<string>(ActionCommon.Subject); }
            set { Attributes[ActionCommon.Subject] = value; }
        }

        public string Description
        {
            get { return GetAttributeValue<string>(ActionCommon.Description); }
            set { Attributes[ActionCommon.Description] = value; }
        }

        public DateTime? ScheduledStart
        {
            get { return GetAttributeValue<DateTime?>(ActionCommon.Sch_start); }
            set { Attributes[ActionCommon.Sch_start] = value; }
        }

        public DateTime? ScheduledEnd
        {
            get { return GetAttributeValue<DateTime?>(ActionCommon.Sch_end); }
            set { Attributes[ActionCommon.Sch_end] = value; }
        }

        public DateTime? SendOn
        {
            get { return GetAttributeValue<DateTime?>(ActionCommon.SendOn); }
            set { Attributes[ActionCommon.SendOn] = value; }
        }

        public DateTime? ActualStart
        {
            get { return GetAttributeValue<DateTime?>(ActionCommon.ActualStart); }
            set { Attributes[ActionCommon.ActualStart] = value; }
        }

        public DateTime? ActualEnd
        {
            get { return GetAttributeValue<DateTime?>(ActionCommon.ActualEnd); }
            set { Attributes[ActionCommon.ActualEnd] = value; }
        }

        public EntityReference RegardingObjectId
        {
            get { return GetAttributeValue<EntityReference>(ActionCommon.RegardingObjectId); }
            set { Attributes[ActionCommon.RegardingObjectId] = value; }
        }

        public string RegardingObjectIdName
        {
            get { return GetAttributeValue<string>(ActionCommon.RegardingObjectIdName); }
            set { Attributes[ActionCommon.RegardingObjectIdName] = value; }
        }
    }
}
