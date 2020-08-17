using System;
namespace ConsoleNotificationDispatcher.Settings
{
    public static class DispatcherConstants
    {
        public static string[] SubscriptionTags { get; set; } = { "default" };

        //TODO: Update
        public static string NotificationHubName { get; set; } = "POCHubNotifications";
        //TODO: Update
        public static string FullAccessConnectionString { get; set; } = "Endpoint=sb://pochubnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=JTY/cE9MdxNOtkCzBEUFST8YYluIVhEQDvUGmpVBip0=";
    }
}
