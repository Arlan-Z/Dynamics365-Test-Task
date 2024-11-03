using AwaraIT.Training.Domain.Models.Crm;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace AwaraIT.Training.Infrastucture.Sources.Crm.Clients
{
    public class LogClient : CrmBaseClient<Log>
    {
        public LogClient(IOrganizationService service) : base(service) { }

        protected override string EntityName => Log.EntityLogicalName;

        internal List<Log> GetLogsBySubjectAndCreatedOn(DateTime dateTime, params string[] names)
        {
            var query = new QueryExpression
            {
                EntityName = Log.EntityLogicalName,
                NoLock = true,
                Criteria =
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                        new ConditionExpression (Log.Metadata.Subject, ConditionOperator.In, names),
                        new ConditionExpression (EntityCommon.CreatedOn, ConditionOperator.OnOrAfter, dateTime)
                    }
                },

            };

            var logs = this.GetAllPaged(query);
            return logs;
        }
    }
}
