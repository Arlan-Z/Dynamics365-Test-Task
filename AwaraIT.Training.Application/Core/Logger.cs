using AwaraIT.Training.Domain.Extensions;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Infrastucture.Sources.Crm.Clients;
using Microsoft.Xrm.Sdk;
using System;

namespace AwaraIT.Training.Application.Core
{
    public class Logger
    {
        private readonly LogClient _logClient;

        public Logger(IOrganizationService service)
        {
            _logClient = new LogClient(service);
        }

        public Guid TRACE(string subject, string description = null, string entityType = null, Guid? entityid = null)
        {
            return WriteToLog(subject, Log.Metadata.LevelOptions.TRACE, description, entityType, entityid);
        }

        public Guid DEBUG(string subject, string description = null, string entityType = null, Guid? entityid = null)
        {
            return WriteToLog(subject, Log.Metadata.LevelOptions.DEBUG, description, entityType, entityid);
        }

        public Guid INFO(string subject, string description = null, string entityType = null, Guid? entityid = null)
        {
            return WriteToLog(subject, Log.Metadata.LevelOptions.INFO, description, entityType, entityid);
        }

        public Guid WARNING(string subject, string description = null, string entityType = null, Guid? entityid = null)
        {
            return WriteToLog(subject, Log.Metadata.LevelOptions.WARNING, description, entityType, entityid);
        }

        public Guid ERROR(string subject, string description = null, string entityType = null, Guid? entityid = null)
        {
            return WriteToLog(subject, Log.Metadata.LevelOptions.ERROR, description, entityType, entityid);
        }

        public Guid ERROR(Exception ex, string errorPrefix = null, string entityType = null, Guid? entityid = null)
        {
            var subject = errorPrefix == null ? ex.Message : $"{errorPrefix}: {ex.Message}";
            return WriteToLog(subject, Log.Metadata.LevelOptions.ERROR, ex.ToString(), entityType, entityid);
        }

        public Guid CRITICAL(string subject, string description = null, string entityType = null, Guid? entityid = null)
        {
            return WriteToLog(subject, Log.Metadata.LevelOptions.CRITICAL, description, entityType, entityid);
        }

        private Guid WriteToLog(string subject, Log.Metadata.LevelOptions level, string description = null, string entityType = null, Guid? entityid = null)
        {
            return _logClient.Create(new Log
            {
                Level = level,
                Subject = subject,
                Description = description.Crop(2000),
                EntityType = entityType,
                EntityId = entityid?.ToString()
            });
        }
    }
}
