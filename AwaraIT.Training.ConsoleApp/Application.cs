using AwaraIT.Training.ConsoleApp.Actions;
using System;
using System.Configuration;

namespace AwaraIT.Training.ConsoleApp
{
    internal static class Application
    {
        internal static void Run()
        {
            try
            {
                Console.WriteLine($"ConnectionString: {ConfigurationManager.ConnectionStrings["Crm"].ConnectionString}");
                Console.WriteLine("Please select action: ");
                foreach (UtilityAction a in Enum.GetValues(typeof(UtilityAction)))
                {
                    Console.WriteLine($"{(int)a} - {a}");
                }

                var taskName = Console.ReadLine();

                if (Enum.TryParse(taskName, true, out UtilityAction action))
                {
                    Console.WriteLine($"Your choice is: {action}. Are you sure? (Y/N)");
                    var input = Console.ReadLine()?.ToUpper();
                    if (input != "Y")
                    {
                        Run();
                    }
                    Console.WriteLine($"Start. Date Time: {DateTime.Now}");
                    switch (action)
                    {
                        case UtilityAction.TestAction:
                            TestAction.Run();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Could not parse required action: {taskName}");
                }

                Console.WriteLine($"Finish. Date Time: {DateTime.Now}");
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }
    }
}
