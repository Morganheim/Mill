using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private AudioData _audioData;
    [SerializeField] private AudioPlayer _sfxPlayerPrefab;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _mixerMusicGroup;
    [SerializeField] private AudioMixerGroup _mixerSFXGroup;

    private HashSet<AudioPlayer> _availableAudioPlayerPool = new();
    private HashSet<AudioPlayer> _activeAudioPlayerPool = new();

    private UnityEvent _onGameStart = new();
    private UnityEvent _onGameClose = new();

    private void OnEnable()
    {
        SetupAudioManager();

        PlayClip(_audioData.MainMenuMusic, _mixerMusicGroup, true, _onGameStart);
    }

    public void OnGameLoaded()
    {
        //stop main menu music
        _onGameStart?.Invoke();

        //play game music
        PlayClip(_audioData.GameMusic, _mixerMusicGroup, true, _onGameClose);
    }

    public void OnMainMenuRequested()
    {
        //stop game music
        _onGameClose?.Invoke();

        //play main menu music
        PlayClip(_audioData.MainMenuMusic, _mixerMusicGroup, true, _onGameStart);
    }

    public void OnGameComplete(GameEventMessage gameEventMessage)
    {
        _onGameClose?.Invoke();
    }

    public void OnSFXRequested(GameEventMessage gameEventMessage)
    {
        AudioMessage message = (AudioMessage)gameEventMessage;
        if (message == null)
            return;

        AudioMixerGroup mixerGroup = message.AudioType.Equals(AudioType.SFX) ? _mixerSFXGroup : _mixerMusicGroup;

        PlayClip(message.Clip, mixerGroup, message.IsLooping, message.OnStopLoopEvent);
    }

    public void OnVolumeChangeRequested(GameEventMessage gameEventMessage)
    {
        AudioMessage message = (AudioMessage)gameEventMessage;
        if (message == null)
            return;

        SetMixerGroupVolume(message.Volume, message.AudioType);
    }

    private void SetupAudioManager()
    {
        GameObject sfxHolder = new();
        sfxHolder.transform.parent = transform;
        sfxHolder.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        sfxHolder.name = "AudioHolder";

        for (int i = 0; i < _gameData.AudioPoolSize; i++)
        {
            AudioPlayer newAudioPlayer = Instantiate(_sfxPlayerPrefab, sfxHolder.transform);
            newAudioPlayer.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            newAudioPlayer.name = $"Audio_Player_{i + 1}";

            _availableAudioPlayerPool.Add(newAudioPlayer);
        }
    }

    private void PlayClip(AudioClip clip, AudioMixerGroup mixerGroup, bool isLooping = false, UnityEvent onStopLoop = null)
    {
        if (_availableAudioPlayerPool.Count < 1)
        {
            Debug.LogError($"Audio player pool is empty! Can't play audio!");
            return;
        }

        AudioPlayer audioPlayer = GetAudioPlayer();
        if (audioPlayer == null)
        {
            Debug.LogError($"Could not find an audio player!");
            return;
        }

        AudioInfo info = new(clip, mixerGroup, isLooping, onStopLoop, ReturnToPool);
        audioPlayer.Play(info);
    }

    private void ReturnToPool(AudioPlayer audioPlayer)
    {
        if (!_availableAudioPlayerPool.Contains(audioPlayer))
            _availableAudioPlayerPool.Add(audioPlayer);

        if (_activeAudioPlayerPool.Contains(audioPlayer))
            _activeAudioPlayerPool.Remove(audioPlayer);
    }

    private void SetMixerGroupVolume(float volume, AudioType audioType)
    {
        string groupKey = audioType switch
        {
            AudioType.SFX => "SFXVolume",
            AudioType.Music => "MusicVolume",
            _ => ""
        };

        _mixer.SetFloat(groupKey, Mathf.Log10(volume) * 20);
    }

    private AudioPlayer GetAudioPlayer()
    {
        AudioPlayer audioPlayer = null;

        foreach(AudioPlayer player in _availableAudioPlayerPool)
        {
            if (player.IsPlaying)
            {
                Debug.LogWarning($"Audio player {player.name} was playing ({player.IsPlaying}) {player.AudioSource.clip.name} while in available pool!\nRemoving from available pool and adding to active pool!");
                _availableAudioPlayerPool.Remove(player);
                _activeAudioPlayerPool.Add(player);
                continue;
            }

            _availableAudioPlayerPool.Remove(player);
            _activeAudioPlayerPool.Add(player);
            audioPlayer = player;
            break;
        }

        return audioPlayer;
    }
}

public struct AudioInfo
{
    public AudioClip Clip { get; private set; }
    public AudioMixerGroup MixerGroup { get; private set; }
    public bool IsLooping { get; private set; }
    public UnityEvent OnStopLoop { get; private set; }
    public System.Action<AudioPlayer> OnCompleteAction { get; private set; }

    public AudioInfo(AudioClip clip, AudioMixerGroup mixerGroup, bool isLooping, UnityEvent onStopLoop, System.Action<AudioPlayer> onCompleteAction)
    {
        Clip = clip;
        MixerGroup = mixerGroup;
        IsLooping = isLooping;
        OnStopLoop = onStopLoop;
        OnCompleteAction = onCompleteAction;
    }

    public AudioInfo(AudioInfo audioInfo)
    {
        Clip = audioInfo.Clip;
        MixerGroup = audioInfo.MixerGroup;
        IsLooping = audioInfo.IsLooping;
        OnStopLoop = audioInfo.OnStopLoop;
        OnCompleteAction = audioInfo.OnCompleteAction;
    }
}

public enum AudioType { Music, SFX }