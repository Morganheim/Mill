using UnityEngine;

[System.Serializable]
public class BoolMessage : GameEventMessage
{
    [field: SerializeField] public bool BoolValue { get; private set; }

    public BoolMessage(bool boolValue)
    {
        BoolValue = boolValue;
    }

    public BoolMessage(string eventName, bool boolValue) : base(eventName)
    {
        BoolValue = boolValue;
    }
}