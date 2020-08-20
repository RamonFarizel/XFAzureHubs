using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using WindowsAzure.Messaging;
using XFAzureHubs.Settings;

namespace XFAzureHubs.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnNewToken(string token)
        {
            // NOTE: save token instance locally, or log if desired

            SendRegistrationToServer(token);
        }

        void SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(AppConstants.NotificationHubName, AppConstants.ListenConnectionString, this);

                // register device with Azure Notification Hub using the token from FCM
                Registration registration = hub.Register(token, AppConstants.SubscriptionTags);

                // subscribe to the SubscriptionTags list with a simple template.
                string pnsHandle = registration.PNSHandle;
                TemplateRegistration templateReg = hub.RegisterTemplate(pnsHandle, "defaultTemplate", AppConstants.FCMTemplateBody, AppConstants.SubscriptionTags);
            }
            catch (Exception e)
            {
                Log.Error(AppConstants.DebugTag, $"Error registering device: {e.Message}");
            }
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;
            string messageTitle = string.Empty;
            string messageType = string.Empty;
            string messageId = string.Empty;

            var messageDictionary = message.Data.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            

            if (messageDictionary.ContainsKey(AppConstants.TITLE_VALUE))
                messageTitle = GetValueFromMessageDictionary(AppConstants.TITLE_VALUE, messageDictionary);

            if (messageDictionary.ContainsKey(AppConstants.BODY_VALUE))
                messageBody = GetValueFromMessageDictionary(AppConstants.BODY_VALUE, messageDictionary);

            if (messageDictionary.ContainsKey(AppConstants.TYPE_VALUE))
                messageType = GetValueFromMessageDictionary(AppConstants.TYPE_VALUE, messageDictionary);

            if (messageDictionary.ContainsKey(AppConstants.ID_VALUE))
                messageId = GetValueFromMessageDictionary(AppConstants.ID_VALUE, messageDictionary);




            //messageBody = message.Data.Values.Last();

            // convert the incoming message to a local notification
            SendLocalNotification(messageTitle, messageBody, messageType, messageId);

            // send the incoming message directly to the MainPage
            SendMessageToMainPage(messageBody);
        }

        string GetValueFromMessageDictionary(string key, Dictionary<string, string> messageDictionary) =>
            messageDictionary.GetValueOrDefault(key, string.Empty);


        void SendLocalNotification(string title, string body, string type, string id)
        {
            Bundle extras = new Bundle();
            extras.PutString(AppConstants.TYPE_VALUE,type);
            extras.PutString(AppConstants.ID_VALUE, id);

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtras(extras);
            


            //Unique request code to avoid PendingIntent collision.
            var requestCode = new Random().Next();
            var pendingIntent = PendingIntent.GetActivity(this, requestCode, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(AppConstants.NotificationChannelName);
            }

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        void SendMessageToMainPage(string body)
        {
            Log.Debug(AppConstants.DebugTag, body);
        }
    }

}
