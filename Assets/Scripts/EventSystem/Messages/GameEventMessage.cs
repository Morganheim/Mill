public class GameEventMessage
{
    public string EventName { get; private set; }

    public GameEventMessage()
    {

    }

    public GameEventMessage(string eventName)
    {
        EventName = eventName;
    }
}