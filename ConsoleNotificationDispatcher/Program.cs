﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConsoleNotificationDispatcher.NotificationModel;
using ConsoleNotificationDispatcher.NotificationModel.Android;
using ConsoleNotificationDispatcher.NotificationModel.IOS;
using ConsoleNotificationDispatcher.Settings;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;

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
                    var messageAndroid = JsonConvert.SerializeObject(new PayloadAndroid(new Data("Message Title", "Message Body")));
                    await hub.SendFcmNativeNotificationAsync(messageAndroid, tag);

                    var messageiOS = JsonConvert.SerializeObject(new PayloadIOS(new Aps(new Alert("Message Title","Message Body"))));
                    await hub.SendAppleNativeNotificationAsync(messageiOS, tag);


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

    }
}
