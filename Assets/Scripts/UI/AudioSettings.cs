using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData _gameData;

    [Header("Emitter")]
    [SerializeField] private GameEventEmitter _emitter;

    [Header("Data")]
    [SerializeField] private Slider _musicVolumeSliderController;
    [SerializeField] private Slider _sfxVolumeSliderController;

    public bool IsEnabled { get; private set; }

    public void OnMusicSliderValueChange(float value)
    {
        _gameData.UpdateMusicVolume(value);
        _emitter.Emit(new AudioMessage("OnVolumeChangeRequested", AudioType.Music, null, false, null, value));
    }

    public void OnSFXSliderValueChange(float value)
    {
        _gameData.UpdateSFXVolume(value);
        _emitter.Emit(new AudioMessage("OnVolumeChangeRequested", AudioType.SFX, null, false, null, value));
    }

    public void OnGameLoaded()
    {
        SetSliders();
    }

    public void OnMainMenuRequested()
    {
        SetSliders();
    }

    public void ToggleEnable()
    {
        IsEnabled = !IsEnabled;
    }

    private void SetSliders()
    {
        _musicVolumeSliderController.value = _gameData.MusicVolume;
        _sfxVolumeSliderController.value = _gameData.SFXVolume;
    }
}
