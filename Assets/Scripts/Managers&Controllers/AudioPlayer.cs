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

        if (_audioInfo.IsLooping && _audioInfo.OnStopLoop != null)
            _audioInfo.OnStopLoop.AddListener(OnLoopStop);

        AudioSource.Play();

        if (!_audioInfo.IsLooping)
            StartCoroutine(CheckAudioFinishedPlaying());
    }

    private void StopPlaying()
    {
        AudioSource.Stop();

        AudioSource.clip = null;
        //AudioSource.volume = 0;
        AudioSource.loop = false;

        _audioInfo.OnCompleteAction(this);
    }

    private void OnLoopStop()
    {
        AudioSource.loop = false;

        _audioInfo.OnStopLoop.RemoveListener(OnLoopStop);

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
