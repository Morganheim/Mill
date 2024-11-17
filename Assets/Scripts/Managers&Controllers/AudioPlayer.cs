using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    private AudioInfo _audioInfo;

    public bool IsPlaying => AudioSource.isPlaying;

    public void Play(AudioInfo audioInfo)
    {
        _audioInfo = new(audioInfo);

        AudioSource.outputAudioMixerGroup = _audioInfo.MixerGroup;
        AudioSource.clip = _audioInfo.Clip;
        AudioSource.loop = _audioInfo.IsLooping;

        _audioInfo.OnStopEvent?.AddListener(OnStopEvent);

        AudioSource.Play();

        if (!_audioInfo.IsLooping)
            StartCoroutine(CheckAudioFinishedPlaying());
    }

    public void ForceStop()
    {
        StopPlaying();
    }

    private void StopPlaying()
    {
        AudioSource.Stop();

        AudioSource.clip = null;
        AudioSource.loop = false;

        _audioInfo.OnCompleteAction(this);
    }

    private void OnStopEvent()
    {
        AudioSource.loop = false;

        _audioInfo.OnStopEvent.RemoveListener(OnStopEvent);

        //stops audio immediately
        StopPlaying();
    }

    private IEnumerator CheckAudioFinishedPlaying()
    {
        yield return new WaitUntil(() => !AudioSource.isPlaying);

        //return to pool
        StopPlaying();
    }
}
