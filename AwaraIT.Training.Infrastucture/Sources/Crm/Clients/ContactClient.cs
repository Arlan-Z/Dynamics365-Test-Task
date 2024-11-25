using AwaraIT.Training.Domain.Models.Crm.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace AwaraIT.Training.Infrastucture.Sources.Crm.Clients
{
    public class ContactClient : CrmBaseClient<Contact>
    {
        public ContactClient(IOrganizationService service) : base(service) { }

        protected override string EntityName => Contact.EntityLogicalName;

        public Contact GetByFullName(string fullName)
        {
            var res = Get(new[] { new ConditionExpression(Contact.Metadata.MiddleName, ConditionOperator.Equal, fullName) }, 1,
                Contact.Metadata.FirstName, Contact.Metadata.LastName);
            return res.FirstOrDefault();
        }
    }
}
