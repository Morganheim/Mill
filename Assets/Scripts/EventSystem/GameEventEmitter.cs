using System.Collections.Generic;
using UnityEngine;

public class GameEventEmitter : MonoBehaviour
{
    [SerializeField] private List<EventEmitterResponse> _emitterResponses;

    private void OnEnable()
    {
        for (int i = 0; i < _emitterResponses.Count; i++)
            _emitterResponses[i].SubscribeEmitter(this);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _emitterResponses.Count; i++)
            _emitterResponses[i].UnsubscribeEmitter(this);
    }

    public void EmitAll(GameEventMessage message = null)
    {
        for (int i = 0; i < _emitterResponses.Count; i++)
            _emitterResponses[i].Emit(this, message);
    }

    public void Emit(GameEventMessage message)
    {
        for (int i = 0; i < _emitterResponses.Count; i++)
        {
            if (_emitterResponses[i].ResponseName != message.EventName)
                continue;

            _emitterResponses[i].Emit(this, message);
        }
    }

    public void Emit(string name)
    {
        for (int i = 0; i < _emitterResponses.Count; i++)
        {
            if (_emitterResponses[i].ResponseName != name)
                continue;

            _emitterResponses[i].Emit(this, null);
        }
    }
}


[System.Serializable]
public struct EventEmitterResponse
{
    [field: SerializeField] public string ResponseName { get; private set; }
    [SerializeField] private List<GameEvent> _scriptableEvents;

    public void SubscribeEmitter(GameEventEmitter emitter)
    {
        for (int i = 0; i < _scriptableEvents.Count; i++)
            _scriptableEvents[i].SubscribeEmitter(emitter);
    }

    public void UnsubscribeEmitter(GameEventEmitter emitter)
    {
        for (int i = 0; i < _scriptableEvents.Count; i++)
            _scriptableEvents[i].UnsubscribeEmitter(emitter);
    }

    public void Emit(GameEventEmitter emitter, GameEventMessage message)
    {
        for (int i = 0; i < _scriptableEvents.Count; i++)
            _scriptableEvents[i].Emit(emitter, message);
    }
}