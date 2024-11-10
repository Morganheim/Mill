using UnityEngine;

[System.Serializable]
public class StringMessage : GameEventMessage
{
    [field: SerializeField] public string StringValue { get; private set; }

    public StringMessage(string stringValue)
    {
        StringValue = stringValue;
    }

    public StringMessage(string eventName, string stringValue) : base(eventName)
    {
        StringValue = stringValue;
    }
}