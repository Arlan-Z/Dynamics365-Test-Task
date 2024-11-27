using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Plugins.PluginExtensions;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
using static AwaraIT.Training.Domain.Models.Crm.Entities.Deal.Metadata;
using static AwaraIT.Training.Domain.Models.Crm.Entities.Interest.Metadata;

namespace AwaraIT.Arlan.Plugins.PossibleDeal
{
    public class DealCreatePlugin : PluginBase
    {
        public DealCreatePlugin() 
        {
            Subscribe
                .ToMessage(CrmMessage.Update)
                .ForEntity(Interest.EntityLogicalName)
                .WithAnyField(Interest.Metadata.Status)
                .When(PluginStage.PostOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);

            try
            {
                var interestEntity = wrapper.TargetEntity.ToEntity<Interest>();
                if (interestEntity.Status != InterestStatusOptions.В_работе)
                {
                    return;
                }

                if (interestEntity == null) throw new Exception("Interest Entity is NULL");

                var interestPre = wrapper.PreImage.ToEntity<Interest>();

                if (interestPre == null) throw new Exception("Interest Pre Image is NULL");

                var newDeal = new Entity(Deal.EntityLogicalName)
                {
                    [Deal.Metadata.RegionId] = interestPre.RegionId,
                    [Deal.Metadata.ContactId] = interestPre.ContactId,
                };

                wrapper.Service.Create(newDeal);
            }
            catch (Exception ex)
            {
                logger.ERROR("Plugin.Arlan DealCreatePlugin",
                    ex.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
            }
        }
    }
}
