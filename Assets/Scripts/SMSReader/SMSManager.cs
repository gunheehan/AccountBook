using System;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;

public class SMSManager : MonoBehaviour
{
    private void Start()
    {
        #if UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = "354",
            Name = "AccountBank Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        #endif
    }
    
    public void OnSMSReceive (string message) 
    {
#if UNITY_ANDROID
        PushNotificationAndriod(message);
#elif UNITY_IOS
        PushNotificationIOS(message);
#endif
    }

    private void PushNotificationAndriod(string msg)
    {
#if UNITY_ANDROID
        try
        {
            var notification = new AndroidNotification();
            notification.Title = "AccountBank";
            notification.Text = msg;
            notification.FireTime = DateTime.Now;

            notification.ShowTimestamp = true;
            notification.ShouldAutoCancel = true;

            AndroidNotificationCenter.SendNotification(notification, "354");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        #endif
    }

    private void PushNotificationIOS(string msg)
    {
        #if UNITY_IOS
        try
        {
            var timetrigger = new iOSNotificationTimeIntervalTrigger();
            timetrigger.TimeInterval = new TimeSpan(0, 0, 0, 3);
            timetrigger.Repeats = false;

            var notification = new iOSNotification()
            {
                Identifier = "_notification",
                Title = "AccountBank",
                Body = msg,
                ShowInForeground = false,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timetrigger
            };

            iOSNotificationCenter.ScheduleNotification(notification);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
#endif
    }
}
