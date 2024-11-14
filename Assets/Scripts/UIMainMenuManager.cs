using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator _backgroundAnimator;

    [SerializeField] private Image _mainMenuPanel;
    [SerializeField] private Image _settingsPanel;
    [SerializeField] private Image _interactionBlocker;

    [SerializeField] private GameEventEmitter _emitter;

    public void OnGameStartButtonClicked()
    {
        _emitter.Emit("OnGameStartRequested");

        _interactionBlocker.raycastTarget = false;

        _mainMenuPanel.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
    }

    public void OnSettingsPanelOpenRequested()
    {

    }

    public void OnSettingsPanelCloseRequested()
    {

    }

    public void OnMainMenuRequested()
    {
        _interactionBlocker.raycastTarget = false;

        _mainMenuPanel.gameObject.SetActive(true);
        _settingsPanel.gameObject.SetActive(true);

        _backgroundAnimator.Play("BackgroundAnim");
    }

    public void OnQuitGameRequested()
    {
        _emitter.Emit("OnQuitGameRequested");
    }
}
