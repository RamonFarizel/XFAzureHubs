﻿using System;
namespace XFAzureHubs.Settings
{
    public static class AppConstants
    {
        public static string NotificationChannelName { get; set; } = "XamarinNotifyChannel";
        public static string NotificationHubName { get; set; } = "POCHubNotifications";
        public static string ListenConnectionString { get; set; } = "Endpoint=sb://pochubnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=dFbV3PF/TEW6MoMLNstUMKDkchc1pnoxh5Rsj3+J98c=";
        public static string DebugTag { get; set; } = "XamarinNotify";
        public static string[] SubscriptionTags { get; set; } = { "default" };
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";
    }
}
