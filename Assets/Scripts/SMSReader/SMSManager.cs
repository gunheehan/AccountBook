using System;
using Unity.Notifications.Android;
using UnityEngine;

public class SMSManager : MonoBehaviour
{
    private void Start()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "354",
            Name = "AccountBank Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    
    public void OnSMSReceive (string message) {
        var notification = new AndroidNotification();
        notification.Title = "AccountBank";
        notification.Text = message;
        notification.FireTime = DateTime.Now;

        notification.ShowTimestamp = true;
        notification.ShouldAutoCancel = true;
        
        AndroidNotificationCenter.SendNotification(notification, "354");
    }
}
