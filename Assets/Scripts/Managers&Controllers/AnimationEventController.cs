using UnityEngine;
using UnityEngine.Events;

public class AnimationEventController : MonoBehaviour
{
    [SerializeField] private AnimationEventCallback[] _callbacks;

    public void TriggerAnimationEvent(string callbackId)
    {
        if (_callbacks == null || _callbacks.Length < 1)
            return;

        for (int i = 0; i < _callbacks.Length; i++)
        {
            if (_callbacks[i].Id != callbackId)
                continue;

            _callbacks[i].AnimationEvent.Invoke();
        }
    }

    public void TriggerAnimationEvent(int index)
    {
        if (_callbacks == null || _callbacks.Length < 1)
            return;

        _callbacks[index].AnimationEvent.Invoke();
    }
}

[System.Serializable]
public struct AnimationEventCallback
{
    [field: SerializeField] public string Id;
    [field: SerializeField] public UnityEvent AnimationEvent;
}