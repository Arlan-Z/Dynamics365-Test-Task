using AwaraIT.Training.Application.Core;
using Microsoft.Xrm.Sdk;

namespace AwaraIT.Training.Application.Contacts
{
    public class ContactService
    {
        private readonly IOrganizationService _service;
        private readonly Logger _logger;

        public ContactService(IOrganizationService service, Logger logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}
