using AwaraIT.Arlan.Plugins.Services;
using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Plugins.PluginExtensions;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AwaraIT.Training.Domain.Models.Crm.Entities.Deal.Metadata;

namespace AwaraIT.Arlan.Plugins.PossibleDeal
{
    public class AssignUserPlugin : PluginBase
    {
        public AssignUserPlugin() 
        {
            Subscribe
                .ToMessage(CrmMessage.Create)
                .ForEntity(Deal.EntityLogicalName)
                .When(PluginStage.PreOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);
            var deal = wrapper.TargetEntity.ToEntity<Deal>();
            try
            {
                var teams = TeamsService.FindTeamsIdByRegionId("Managers", deal.RegionId.Id, wrapper);

                Entity leastLoadedUser = null;
                int minWorkload = int.MaxValue;

                foreach (var team in teams.Entities)
                {
                    var teamId = team.Id;
                    var dealQuery = new QueryExpression("arl_possibledeal")
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
                                        new ConditionExpression("arl_status", ConditionOperator.Equal, (int)DealStatusOptions.В_работе)
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
                    dealQuery.AddLink("teammembership", "ownerid", "systemuserid");
                    var dealRes = wrapper.Service.RetrieveMultiple(dealQuery);

                    var workloadByUser = dealRes.Entities
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

                if (leastLoadedUser == null)
                {
                    throw new Exception("No suitable user found for the deal assignment.");
                }

                deal.OwnerId = new EntityReference("systemuser", leastLoadedUser.Id);
            }
            catch (Exception ex)
            {
                logger.ERROR(
                    "Plugin.Arlan Assign Deal To User Plugin",
                    ex.ToString(),
                    wrapper.TargetEntity.LogicalName,
                    wrapper.TargetEntity.Id
                );
            }
        }
    }
}
