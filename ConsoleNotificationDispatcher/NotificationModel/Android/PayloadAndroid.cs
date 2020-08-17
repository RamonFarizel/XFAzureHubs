using System;
using Newtonsoft.Json;

namespace ConsoleNotificationDispatcher.NotificationModel.Android
{
    public class PayloadAndroid
    {
        [JsonProperty(PropertyName = "data")]
        public Data Data { get; set; }

        public PayloadAndroid(Data data)
        {
            Data = data;
        }
    }
}
