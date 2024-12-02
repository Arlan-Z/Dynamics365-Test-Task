using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwaraIT.Arlan.Plugins.Services
{
    public static class TeamsService
    {

        private static readonly Dictionary<string, string> Units = new Dictionary<string, string>()
        {
            { "Call-Centre", "becaf010-bba8-ef11-8a6a-000d3a5c09a6" },
            { "Managers", "c49ef665-bba8-ef11-8a6a-000d3a5c09a6" }
        };
        /// <summary>
        /// Finds the teams EntityCollection based on the specified unit name and region ID.
        /// </summary>
        /// <param name="unitName">
        /// The name of the business unit for which the team ID is being searched. 
        /// Must match one of the predefined keys in the <see cref="Units"/> dictionary.
        /// </param>
        /// <param name="regionId">
        /// The GUID of the region to which the team is associated.
        /// </param>
        /// <param name="wrapper">
        /// The context wrapper providing access to the CRM service.
        /// </param>
        /// <returns>
        /// The GUID of the first team matching the provided business unit and region.
        /// </returns>
        /// <example>
        /// Example usage:
        /// <code>
        /// var teamId = TeamsService.FindTeamIdByRegion("Managers", deal.RegionId.Id, wrapper);
        /// </code>
        /// </example>
        public static EntityCollection FindTeamsIdByRegionId(string unitName, Guid regionId, IContextWrapper wrapper)
        {
            var query_regionData = regionId;
            var query_unitData = new Guid(Units[unitName]);

            var teamQuery = new QueryExpression("team")
            {
                ColumnSet = new ColumnSet(true)
            };

            teamQuery.Criteria.AddCondition("businessunitid", ConditionOperator.Equal, query_unitData);

            var regionDataLink = teamQuery.AddLink("arl_regiondata_team", "teamid", "teamid", JoinOperator.Inner);
            regionDataLink.LinkCriteria.AddCondition("arl_regiondataid", ConditionOperator.Equal, query_regionData);

            var teamRes = wrapper.Service.RetrieveMultiple(teamQuery);
            if (teamRes.Entities.Count > 0) return teamRes;
            else throw new Exception("No Team Found");
        }
    }
}
