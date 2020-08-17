using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Foundation;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;
using XFAzureHubs.Settings;

namespace XFAzureHubs.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            UNUserNotificationCenter.Current.Delegate = new NotificationCenterDelegate();

            LoadApplication(new App());

            
            RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
            

        }

        void RegisterForRemoteNotifications()
        {
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Sound |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var Hub = new SBNotificationHub(AppConstants.ListenConnectionString, AppConstants.NotificationHubName);

            // update registration with Azure Notification Hub
            Hub.UnregisterAll(deviceToken, (error) =>
            {
                if (error != null)
                {
                    Debug.WriteLine($"Unable to call unregister {error}");
                    return;
                }

                var tags = new NSSet(AppConstants.SubscriptionTags.ToArray());
                Hub.RegisterNative(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        Debug.WriteLine($"RegisterNativeAsync error: {errorCallback}");
                    }
                });

                var templateExpiration = DateTime.Now.AddDays(120).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                Hub.RegisterTemplate(deviceToken, "defaultTemplate", AppConstants.APNTemplateBody, templateExpiration, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        if (errorCallback != null)
                        {
                            Debug.WriteLine($"RegisterTemplateAsync error: {errorCallback}");
                        }
                    }
                });
            });
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            base.FailedToRegisterForRemoteNotifications(application, error);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // make sure we have a payload
            if (options != null && options.ContainsKey(new NSString("aps")))
            {
                // get the APS dictionary and extract message payload. Message JSON will be converted
                // into a NSDictionary so more complex payloads may require more processing
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                string payload = string.Empty;
                NSString payloadKey = new NSString("alert");
                if (aps.ContainsKey(payloadKey))
                {
                    payload = aps[payloadKey].ToString();
                }

                if (!string.IsNullOrWhiteSpace(payload))
                {
                    Debug.WriteLine(payload);
                }

            }
            else
            {
                Debug.WriteLine($"Received request to process notification but there was no payload.");
            }
        }
    }

    [Preserve(AllMembers = true)]
    public class NotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center,
            UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);

            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
            completionHandler(UNNotificationPresentationOptions.Alert |
                              UNNotificationPresentationOptions.Badge |
                              UNNotificationPresentationOptions.Sound);
        }
    }
}
