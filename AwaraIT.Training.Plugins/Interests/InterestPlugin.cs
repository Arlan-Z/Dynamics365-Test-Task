using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Plugins.PluginExtensions;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace AwaraIT.Training.Plugins.Interests
{
    public class InterestPlugin : PluginBase
    {
        private IContextWrapper wrapper;
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
                var interest = wrapper.TargetEntity.ToEntity<Interest>();
                interest.ContactId = new EntityReference(Contact.EntityLogicalName, FindContact(interest));
                
                
            }
            catch (Exception ex)
            {
                logger.ERROR("Plugin Arlan Interest",
                    ex.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
            }
        }

        private System.Guid FindContact(Interest interest)
        {
            var query = new QueryExpression(Contact.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true)
            };
            query.Criteria.AddCondition(Contact.Metadata.Email, ConditionOperator.Equal, interest.Email);
            query.Criteria.AddCondition(Contact.Metadata.PhoneNumber, ConditionOperator.Equal, interest.PhoneNumber);
            var contacts = wrapper.Service.RetrieveMultiple(query);
            if(contacts.Entities.Count > 0) return contacts.Entities.First().Id;
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

        private void SetOwnerId(Interest interest)
        {
            // finding team by region
            QueryExpression teamQuery = new QueryExpression("team")
            {
                ColumnSet = new ColumnSet("teamid"),
                LinkEntities =
            {
                new LinkEntity
                {
                    LinkFromEntityName = "team",
                    LinkFromAttributeName = "teamid",
                    LinkToEntityName = "arl_regionData_Team_Team",
                    LinkToAttributeName = "teamid",
                    LinkEntities =
                    {
                        new LinkEntity
                        {
                            LinkFromEntityName = "arl_regiondata_team",
                            LinkFromAttributeName = "regiondataid",
                            LinkToEntityName = "arl_regiondata",
                            LinkToAttributeName = "regiondataid",
                            LinkCriteria = new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression("regiondataid", ConditionOperator.Equal, interest.RegionId)
                                }
                            },
                            LinkEntities =
                            {
                                new LinkEntity
                                {
                                    LinkFromEntityName = "arl_regiondata",
                                    LinkFromAttributeName = "businessunitid", // Рабочая единица территории
                                    LinkToEntityName = "businessunit",
                                    LinkToAttributeName = "businessunitid",
                                    LinkCriteria = new FilterExpression
                                    {
                                        Conditions =
                                        {
                                            new ConditionExpression("businessunitid", ConditionOperator.Equal, businessUnitId)
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            }
    }
}
