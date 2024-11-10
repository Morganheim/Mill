using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Mill/Scriptable Events/Game Event")]
public class GameEvent : ScriptableObject
{
    [SerializeField] private List<GameEventEmitter> _emitters = new List<GameEventEmitter>();
    [SerializeField] private List<GameEventListener> _listeners = new List<GameEventListener>();

    public void SubscribeEmitter(GameEventEmitter emitter)
    {
        if (emitter == null || _emitters.Contains(emitter))
            return;

        _emitters.Add(emitter);
    }

    public void UnsubscribeEmitter(GameEventEmitter emitter)
    {
        if (!_emitters.Contains(emitter))
            return;

        _emitters.Remove(emitter);
    }

    public void SubscribeListener(GameEventListener listener)
    {
        if (listener == null || _listeners.Contains(listener))
            return;

        _listeners.Add(listener);
    }

    public void UnsubscribeListener(GameEventListener listener)
    {
        if (!_listeners.Contains(listener))
            return;

        _listeners.Remove(listener);
    }

    public void Emit(GameEventEmitter emitter, GameEventMessage message = null)
    {
        if (emitter == null)
            return;

        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i].OnEventEmitted(this, emitter, message);
    }
}