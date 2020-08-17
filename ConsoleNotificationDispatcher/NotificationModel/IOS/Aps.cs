using System;
using Newtonsoft.Json;

namespace ConsoleNotificationDispatcher.NotificationModel.IOS
{
    public class Aps
    {
        [JsonProperty(PropertyName = "alert")]
        public Alert Alert { get; set; }

        public Aps(Alert alert)
        {
            Alert = alert;
        }
    }
}
