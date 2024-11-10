using UnityEngine;

[System.Serializable]
public class FloatMessage : GameEventMessage
{
    [field: SerializeField] public float FloatValue { get; private set; }

    public FloatMessage(float floatValue)
    {
        FloatValue = floatValue;
    }

    public FloatMessage(string eventName, float floatValue) : base(eventName)
    {
        FloatValue = floatValue;
    }
}