using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Plugins.PluginExtensions;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using static AwaraIT.Training.Domain.Models.Crm.Entities.Interest.Metadata;

namespace AwaraIT.Training.Plugins.Interests
{
    public class InterestPlugin : PluginBase
    {
        private IContextWrapper wrapper;
        private Interest interest;

        public InterestPlugin()
        {
            Subscribe
                .ToMessage(CrmMessage.Create)
                .ForEntity(Interest.EntityLogicalName)
                .When(PluginStage.PreOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);
            this.wrapper = wrapper;

            try
            {
                this.interest = wrapper.TargetEntity.ToEntity<Interest>();
                interest.ContactId = new EntityReference(Contact.EntityLogicalName, FindContact());
                SetOwner();
            }
            catch (Exception ex)
            {
                logger.ERROR(
                    "Plugin.Arlan InterestPlugin",
                    ex.ToString(),
                    wrapper.TargetEntity.LogicalName,
                    wrapper.TargetEntity.Id
                );
            }
        }

        private Guid FindContact()
        {
            var query = new QueryExpression(Contact.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true)
            };

            query.Criteria.AddCondition(Contact.Metadata.Email, ConditionOperator.Equal, interest.Email);
            query.Criteria.AddCondition(Contact.Metadata.PhoneNumber, ConditionOperator.Equal, interest.PhoneNumber);

            var contacts = wrapper.Service.RetrieveMultiple(query);

            if (contacts.Entities.Count > 0)
            {
                return contacts.Entities.First().Id;
            }
            else
            {
                var newContact = new Entity(Contact.EntityLogicalName)
                {
                    [Contact.Metadata.FirstName] = interest.FirstName,
                    [Contact.Metadata.MiddleName] = interest.MiddleName,
                    [Contact.Metadata.LastName] = interest.LastName,
                    [Contact.Metadata.Email] = interest.Email,
                    [Contact.Metadata.PhoneNumber] = interest.PhoneNumber,
                    [Contact.Metadata.RegionId] = interest.RegionId
                };

                return wrapper.Service.Create(newContact);
            }
        }

        private void SetOwner()
        {
            var query_arl_regiondata_team_arl_regiondataid = interest.RegionId.Id;
            var query_businessunitid = new Guid("becaf010-bba8-ef11-8a6a-000d3a5c09a6");

            var teamQuery = new QueryExpression("team")
            {
                ColumnSet = new ColumnSet(true) 
            };

            teamQuery.Criteria.AddCondition("businessunitid", ConditionOperator.Equal, query_businessunitid);

            var regionDataLink = teamQuery.AddLink("arl_regiondata_team", "teamid", "teamid", JoinOperator.Inner);
            regionDataLink.LinkCriteria.AddCondition("arl_regiondataid", ConditionOperator.Equal, query_arl_regiondata_team_arl_regiondataid);

            var teamRes = wrapper.Service.RetrieveMultiple(teamQuery);
            if (teamRes.Entities.Count > 0)
            {
                var teamId = teamRes.Entities.First().Id;
                var userQuery = new QueryExpression("systemuser");

                userQuery.ColumnSet.AllColumns = true;
                userQuery.Criteria.AddCondition("teammembership", "teamid", ConditionOperator.Equal, teamId);
                userQuery.AddLink("teammembership", "systemuserid", "systemuserid");
                //userQuery.Criteria.AddCondition("systemuserid", ConditionOperator.NotNull, null);

                //var userLink = userQuery.AddLink("teammembership", "systemuserid", "systemuserid", JoinOperator.Inner);
                //userLink.LinkCriteria.AddCondition("teamid", ConditionOperator.Equal, teamId);

                //var interestLink = userQuery.AddLink("arl_interest", "systemuserid", "ownerid", JoinOperator.Inner);
                //interestLink.LinkCriteria.AddCondition("statuscode", ConditionOperator.Equal, 1);

                var userRes = wrapper.Service.RetrieveMultiple(userQuery);

                if (userRes.Entities.Count == 0)
                {
                    throw new Exception($"No User Found, TeamId:{teamId}");
                }

                Entity leastLoadedUser = userRes.Entities.First();
                int minWorkload = int.MaxValue;

                foreach (Entity user in userRes.Entities)
                {
                    var workloadQuery = new QueryExpression("arl_interest")
                    {
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("arl_status", ConditionOperator.Equal, (int)InterestStatusOptions.В_работе),
                                new ConditionExpression("ownerid", ConditionOperator.Equal, user.Id)
                            }
                        }
                    };

                    int workload = wrapper.Service.RetrieveMultiple(workloadQuery).Entities.Count;

                    if (workload < minWorkload)
                    {
                        minWorkload = workload;
                        leastLoadedUser = user;
                    }
                }

                interest.OwnerId.Id = leastLoadedUser.Id;
            }
            else
            {
                throw new Exception("No Team Found");
            }
        }
    }
}
