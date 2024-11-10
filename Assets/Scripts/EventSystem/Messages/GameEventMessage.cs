using UnityEngine;

[System.Serializable]
public class GameEventMessage
{
    [field: SerializeField] public string EventName { get; private set; }

    public GameEventMessage()
    {

    }

    public GameEventMessage(string eventName)
    {
        EventName = eventName;
    }
}