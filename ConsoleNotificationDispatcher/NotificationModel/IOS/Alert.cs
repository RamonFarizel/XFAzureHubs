using System;
using Newtonsoft.Json;

namespace ConsoleNotificationDispatcher.NotificationModel.IOS
{
    public class Alert
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        public Alert(string title,string body)
        {
            Title = title;
            Body = body;
        }

    }
}
