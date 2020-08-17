using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConsoleNotificationDispatcher.Settings;
using Microsoft.Azure.NotificationHubs;

namespace ConsoleNotificationDispatcher
{
    class Program
    {
        static int messageCount;

        static void Main(string[] args)
        {
            Console.WriteLine($"Press the spacebar to send a message to each tag in {string.Join(", ", DispatcherConstants.SubscriptionTags)}");
            WriteSeparator();
            while (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                SendTemplateNotificationsAsync().GetAwaiter().GetResult();
            }
        }

        private static async Task SendTemplateNotificationsAsync()
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(DispatcherConstants.FullAccessConnectionString, DispatcherConstants.NotificationHubName);
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();

            messageCount++;

            // Send a template notification to each tag. This will go to any devices that
            // have subscribed to this tag with a template that includes "messageParam"
            // as a parameter
            foreach (var tag in DispatcherConstants.SubscriptionTags)
            {
                try
                {
                    string jsonPayload =
                    "{" +
                            getQuotedString("data") + ":" +
                            "{" +
                                getQuotedString("title") + ":" + getQuotedString("Titulo TESTE") + "," +
                                getQuotedString("body") + ":" + getQuotedString("Mensagem de teste") +
                            "}" +
                    "}";
                    await hub.SendFcmNativeNotificationAsync(jsonPayload, tag);
                    await hub.SendAppleNativeNotificationAsync(jsonPayload, tag);

                    Console.WriteLine($"Sent message to {tag} subscribers.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send template notification: {ex.Message}");
                }
            }

            Console.WriteLine($"Sent messages to {DispatcherConstants.SubscriptionTags.Length} tags.");
            WriteSeparator();
        }

        private static void WriteSeparator()
        {
            Console.WriteLine("==========================================================================");
        }

        static string getQuotedString(string str)
        {
            return "\"" + str + "\"";
        }

    }
}
