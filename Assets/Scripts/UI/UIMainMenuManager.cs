using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private AudioData _audioData;
    
    [Header("Internal Components")]
    [SerializeField] private GameEventEmitter _emitter;
    [SerializeField] private Animator _backgroundAnimator;
    [SerializeField] private Animator _settingsPanelAnimator;

    [Header("Panels")]
    [SerializeField] private Image _mainMenuPanel;

    public void OnGameStartButtonClicked()
    {
        _settingsPanelAnimator.Play("Idle");

        _mainMenuPanel.gameObject.SetActive(false);

        _emitter.Emit("OnGameStartRequested");
    }

    public void OnSettingsPanelOpenRequested()
    {
        _settingsPanelAnimator.Play("PanelSlideAnimOn");
    }

    public void OnSettingsPanelCloseRequested()
    {
        _settingsPanelAnimator.Play("PanelSlideAnimOff");
    }

    public void OnMainMenuRequested()
    {
        _mainMenuPanel.gameObject.SetActive(true);

        _backgroundAnimator.Play("BackgroundAnim");
        _settingsPanelAnimator.Play("Idle");
    }

    public void OnQuitGameRequested()
    {
        _emitter.Emit("OnQuitGameRequested");
    }

    public void ButtonClickSFX()
    {
        _emitter.Emit(new AudioMessage("OnSFXRequested", AudioType.SFX, _audioData.ButtonClickSFX, false, null));
    }
}
