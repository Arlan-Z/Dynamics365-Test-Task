using Microsoft.Crm.Sdk.Messages;
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
                    var whoAmIRequest = new WhoAmIRequest();
                    var currentUser = (WhoAmIResponse)client.Execute(whoAmIRequest);
                    Console.WriteLine($"UserId: {currentUser.UserId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }
    }
}
