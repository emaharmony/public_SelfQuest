using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_IOS
using Unity.Notifications.iOS;
#else
using Unity.Notifications.Android;
#endif

using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public int hours = 3, mins = 0, seconds = 0;
    public TMPro.TMP_Text text; 
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS
        StartCoroutine(RequestAuthorization());
#endif
#if UNITY_ANDROID
 
    AndroidNotification();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#if UNITY_IOS
    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
            if (req.Granted) NotificationPusher();
        }
    }

    public void NotificationPusher() 
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(hours, mins, seconds),
            Repeats = true
        };

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "_self_quest",
            Title = "Attention Adventurer!",
            Body = "You gotta keep questing! Not for me but for you! The Days is yours!",
            Subtitle = "Just a friendly reminder..",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };
        iOSNotificationCenter.ScheduleNotification(notification);
    }
#endif
#if UNITY_ANDROID
    public void AndroidNotification()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "self_quest",
            Name = "Self-Quest",
            Importance = Importance.Default,
            Description = "The day is yours!",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification();
        notification.Title = "Attention Adventurer!";
        notification.Text = "The Day is yours!";
        notification.FireTime = System.DateTime.Now.AddMinutes((hours * 60 + mins + (seconds/60)));

        AndroidNotificationCenter.SendNotification(notification, "self_quest");
    }
#endif

    public void SetHour(float i)
    {
        hours = (int)i;
        if (text != null)
            text.text = hours + ((i == 1) ? " hour" : " hours");

#if UNITY_IOS
    NotificationPusher();
#endif

#if UNITY_ANDROID
        AndroidNotification();
#endif

    }
}
