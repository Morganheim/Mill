using UnityEngine;
using UnityEngine.Events;

public class AudioMessage : GameEventMessage
{
    public AudioType AudioType { get; private set; }
    public AudioClip Clip { get; private set; }
    public bool IsLooping { get; private set; }
    public UnityEvent OnStopEvent { get; private set; }
    public float Volume { get; private set; }

    public AudioMessage(AudioType audioType, AudioClip clip, bool isLooping, UnityEvent onStopEvent, float volume = 0)
    {
        AudioType = audioType;
        Clip = clip;
        IsLooping = isLooping;
        OnStopEvent = onStopEvent;
        Volume = volume;
    }

    public AudioMessage(string eventName, AudioType audioType, AudioClip clip, bool isLooping, UnityEvent onStopEvent, float volume = 0) : base(eventName)
    {
        AudioType = audioType;
        Clip = clip;
        IsLooping = isLooping;
        OnStopEvent = onStopEvent;
        Volume = volume;
    }
}
