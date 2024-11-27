using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
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
        public static Guid FindTeamByRegion(string unitName, Guid regionId, IContextWrapper wrapper)
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
            if (teamRes.Entities.Count > 0) return teamRes.Entities.First().Id;
            else throw new Exception("No Team Found");
        }
    }
}
