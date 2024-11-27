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
                var teamId = TeamsService.FindTeamByRegion("Managers", deal.RegionId.Id, wrapper);
                var userQuery = new QueryExpression("systemuser");

                userQuery.ColumnSet.AllColumns = true;
                userQuery.Criteria.AddCondition("teammembership", "teamid", ConditionOperator.Equal, teamId);
                userQuery.AddLink("teammembership", "systemuserid", "systemuserid");

                var userRes = wrapper.Service.RetrieveMultiple(userQuery);

                if (userRes.Entities.Count == 0)
                {
                    throw new Exception($"No User Found, TeamId:{teamId}");
                }

                Entity leastLoadedUser = userRes.Entities.First();
                int minWorkload = int.MaxValue;

                foreach (Entity user in userRes.Entities)
                {
                    var workloadQuery = new QueryExpression(Deal.EntityLogicalName)
                    {
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("arl_status", ConditionOperator.Equal, (int)DealStatusOptions.В_работе),
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

                deal.OwnerId.Id = leastLoadedUser.Id;
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
