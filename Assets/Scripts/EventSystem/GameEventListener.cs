using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private List<EventListenerResponse> _eventResponses;

    private void OnEnable()
    {
        for (int i = 0; i < _eventResponses.Count; i++)
            _eventResponses[i].ScriptableEvent.SubscribeListener(this);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _eventResponses.Count; i++)
            _eventResponses[i].ScriptableEvent.UnsubscribeListener(this);
    }

    public void OnEventEmitted(GameEvent sourceEvent, GameEventEmitter emitter, GameEventMessage message = null)
    {
        for (int i = 0; i < _eventResponses.Count; i++)
        {
            if (_eventResponses[i].ScriptableEvent != sourceEvent)
                continue;

            _eventResponses[i].Invoke(message);
        }
    }
}


[System.Serializable]
public struct EventListenerResponse
{
    [field: SerializeField] public GameEvent ScriptableEvent { get; private set; }

    [SerializeField] private UnityMessageEvent _responseEvnet;

    public void Invoke(GameEventMessage message)
    {
        _responseEvnet.Invoke(message);
    }
}


[System.Serializable]
public class UnityMessageEvent : UnityEvent<GameEventMessage> { }