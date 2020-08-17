namespace ConsoleNotificationDispatcher.NotificationModel.Android
{
    public class Data
    {
        public Data(
            string message,
            object payload)
        {
            Message = message;
            Payload = payload;
        }

        public string Message { get; }
        public object Payload { get; }
    }
}
