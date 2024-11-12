using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationMessage : GameEventMessage
{
    public string Message { get; private set; }
    public float NotificationLifetime { get; private set; }

    public NotificationMessage(string message, float notificationLifetime = 0)
    {
        Message = message;
        NotificationLifetime = notificationLifetime;
    }

    public NotificationMessage(string eventName, string message, float notificationLifetime = 0) : base(eventName)
    {
        Message = message;
        NotificationLifetime = notificationLifetime;
    }
}
