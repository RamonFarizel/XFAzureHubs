using System;
using Newtonsoft.Json;

namespace ConsoleNotificationDispatcher.NotificationModel.IOS
{
    public class PayloadIOS
    {
        [JsonProperty(PropertyName = "aps")]
        public Aps Aps { get; set; }

        public PayloadIOS(Aps aps)
        {
            Aps = aps;
        }
    }
}
