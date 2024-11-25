using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace AwaraIT.Training.ConsoleApp.Actions
{
    internal static class TestAction
    {
        internal static void Run()
        {
            try
            {
                using (var client = Program.GetCrmClient())
                {
                    //var whoAmIRequest = new WhoAmIRequest();
                    //var currentUser = (WhoAmIResponse)client.Execute(whoAmIRequest);
                    //Console.WriteLine($"UserId: {currentUser.UserId}");
                    //Console.WriteLine($"Date Now: {DateTime.UtcNow}");

                    var client365 = (IOrganizationService)client;

                    //client365.Create(new Entity
                    //{
                    //    LogicalName = "arl_contacts",
                    //    ["arl_firstname"] = "Egor",
                    //    ["arl_lastname"] = "Polnarev"
                    //});

                    //client365.Update(new Entity
                    //{
                    //    LogicalName = "arl_contacts",
                    //    Id = new Guid("982b4a55-3898-ef11-8a6a-000d3a5c09a6"),
                    //    ["arl_lastname"] = "NeIvanov"
                    //});

                    //client365.Delete("arl_contacts", new Guid("8fcd90bb-7d97-ef11-8a6a-00224805a2d2"));

                    //var name = client365.Retrieve("arl_contacts", new Guid("982b4a55-3898-ef11-8a6a-000d3a5c09a6"), new ColumnSet("arl_fullname"));
                    //Console.WriteLine(name["arl_fullname"]);

                    var query = new QueryExpression("arl_contacts")
                    {
                        ColumnSet = new ColumnSet(true)
                    };
                    query.Criteria.AddCondition(new ConditionExpression("arl_lastname", ConditionOperator.Equal, "NeIvanov"));
                    var res = client365.RetrieveMultiple(query);
                    var contacts = res.Entities;
                    Console.WriteLine(contacts[0]["arl_fullname"]);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }
    }
}
