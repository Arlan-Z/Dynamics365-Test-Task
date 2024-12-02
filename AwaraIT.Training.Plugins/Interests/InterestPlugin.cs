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
using AwaraIT.Arlan.Plugins.Services;

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
                interest.ContactId = new EntityReference(Contact.EntityLogicalName, FindContactId());
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

        private Guid FindContactId()
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
            var teamsRes = TeamsService.FindTeamsIdByRegionId("Call-Centre", interest.RegionId.Id, wrapper);
            Entity leastLoadedUser = null;
            int minWorkload = int.MaxValue;

            foreach (var team in teamsRes.Entities)
            {
                var teamId = team.Id;

                var interestQuery = new QueryExpression("arl_interest")
                {
                    ColumnSet = new ColumnSet("arl_status", "ownerid"),
                    Criteria = new FilterExpression
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression("arl_status", ConditionOperator.Equal, (int)InterestStatusOptions.В_работе)
                                }
                            },
                            new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression("teammembership", "teamid", ConditionOperator.Equal, teamId)
                                }
                            }
                        }
                    }
                };

                interestQuery.AddLink("teammembership", "ownerid", "systemuserid");

                var interestRes = wrapper.Service.RetrieveMultiple(interestQuery);

                var workloadByUser = interestRes.Entities
                    .GroupBy(i => i.GetAttributeValue<EntityReference>("ownerid").Id)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Count()
                    );

                var userQuery = new QueryExpression("systemuser")
                {
                    ColumnSet = new ColumnSet("systemuserid")
                };

                var teamLink = userQuery.AddLink("teammembership", "systemuserid", "systemuserid");
                teamLink.LinkCriteria.AddCondition("teamid", ConditionOperator.Equal, teamId);

                var userRes = wrapper.Service.RetrieveMultiple(userQuery);

                if (userRes.Entities.Count == 0)
                {
                    throw new Exception($"No User Found in Team: {teamId}");
                }

                foreach (var user in userRes.Entities)
                {
                    var userId = user.Id;

                    if (!workloadByUser.ContainsKey(userId))
                    {
                        leastLoadedUser = user;
                        minWorkload = 0;
                        break;
                    }

                    var workload = workloadByUser[userId];
                    if (workload < minWorkload)
                    {
                        minWorkload = workload;
                        leastLoadedUser = user;
                    }
                }

                if (minWorkload == 0) break;
            }
            
            if (leastLoadedUser != null)
            {
                interest.OwnerId = new EntityReference("systemuser", leastLoadedUser.Id);
            }
            else
            {
                throw new Exception("No suitable user found in any team.");
            }
        }
    }
}
