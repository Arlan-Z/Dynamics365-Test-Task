using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Domain.Extensions;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Plugins.PluginExtensions;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using System;

namespace AwaraIT.Training.Plugins.Contacts
{
    public class TestPlugin : PluginBase
    {
        public TestPlugin()
        {
            Subscribe
                .ToMessage(CrmMessage.Update)                
                .ForEntity(Contact.EntityLogicalName)
                .WithAnyField(Contact.Metadata.PhoneNumber)
                .When(PluginStage.PreOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);

            try
            {
                var contact = wrapper.TargetEntity.ToEntity<Contact>();
                //contact.MobilePhone = contact.MobilePhone?.OnlyDigits();
            }
            catch (Exception ex)
            {
                logger.ERROR("Plugin TestPlugin",
                    ex.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
            }
        }
    }
}
