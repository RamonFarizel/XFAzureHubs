using System;
namespace XFAzureHubs.Settings
{
    public static class AppConstants
    {
        public static string NotificationChannelName { get; set; } = "XamarinNotifyChannel";

        //TODO: Update
        public static string NotificationHubName { get; set; } = "POCHubNotifications";
        //TODO: Update
        public static string ListenConnectionString { get; set; } = "Endpoint=sb://pochubnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=dFbV3PF/TEW6MoMLNstUMKDkchc1pnoxh5Rsj3+J98c=";
        

        public static string DebugTag { get; set; } = "XamarinNotify";
        public static string[] SubscriptionTags { get; set; } = { "default" };

        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";


        public static readonly string TITLE_VALUE = "title";
        public static readonly string BODY_VALUE = "body";
        public static readonly string TYPE_VALUE = "type";
        public static readonly string ID_VALUE = "id";
    }
}
