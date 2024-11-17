using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameManager : MonoBehaviour
{
    [Header("Internal Components")]
    [SerializeField] private GameEventEmitter _emitter;

    [Header("Data")]
    [SerializeField] private PlayerData _player1Data;
    [SerializeField] private PlayerData _player2Data;
    [SerializeField] private GameData _gameData;
    [SerializeField] private AudioData _audioData;

    [Header("Panel References")]
    [SerializeField] private Animator _pausePanelAnimator;
    [SerializeField] private Animator _gameOverPanelAnimator;
    [SerializeField] private Animator _audioPanelAnimator;
    [SerializeField] private AudioSettings _audioPanel;
    [SerializeField] private CanvasGroup _pausePanel;
    [SerializeField] private CanvasGroup _gameOverPanel;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _notificationDisplayText;
    [SerializeField] private TextMeshProUGUI _player1TurnDisplayText;
    [SerializeField] private TextMeshProUGUI _player1PiecesDisplayText;
    [SerializeField] private TextMeshProUGUI _player1PiecesValueDisplayText;
    [SerializeField] private TextMeshProUGUI _player2TurnDisplayText;
    [SerializeField] private TextMeshProUGUI _player2PiecesDisplayText;
    [SerializeField] private TextMeshProUGUI _player2PiecesValueDisplayText;
    [SerializeField] private TextMeshProUGUI _millDisplayText;
    [SerializeField] private TextMeshProUGUI _gameStateDisplayText;
    [SerializeField] private TextMeshProUGUI _gameOverNotificationText;
    [SerializeField] private Image _gameBackground;

    private const string ON_TURN_SUFIX = "Playing";
    private const string OFF_TURN_SUFIX = "Waiting";
    private const string STATE_PREFIX = "Phase: ";
    private const string PIECES_PREFIX = "Pieces left: ";
    private const string MILL_TEXT = "MILL!!";

    private Coroutine _typingCoroutine = null;
    private string _cachedNotification;

    public void OnDisplayNotification(GameEventMessage gameEventMessage)
    {
        NotificationMessage notificationMessage = (NotificationMessage)gameEventMessage;
        if (notificationMessage == null)
            return;

        _cachedNotification = notificationMessage.Message;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeMessage(_notificationDisplayText, notificationMessage.Message));
    }

    public void OnDisplayTempNotification(GameEventMessage gameEventMessage)
    {
        NotificationMessage notificationMessage = (NotificationMessage)gameEventMessage;
        if (notificationMessage == null)
            return;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeMessage(_notificationDisplayText, notificationMessage.Message, notificationMessage.NotificationLifetime));
    }

    public void OnGameStateChanged(GameEventMessage gameEventMessage)
    {
        GameStateMessage stateMessage = (GameStateMessage)gameEventMessage;
        if (stateMessage == null)
            return;

        _gameStateDisplayText.text = STATE_PREFIX + stateMessage.CurrentStateType.ToString();

        string player1Action = stateMessage.CurrentPlayer == _player1Data ? ON_TURN_SUFIX : OFF_TURN_SUFIX;
        string player2Action = stateMessage.CurrentPlayer == _player2Data ? ON_TURN_SUFIX : OFF_TURN_SUFIX;

        _player1TurnDisplayText.text = $"{_player1Data}: {player1Action}";
        _player2TurnDisplayText.text = $"{_player2Data}: {player2Action}";

        _player1PiecesDisplayText.text = $"{PIECES_PREFIX}";
        _player2PiecesDisplayText.text = $"{PIECES_PREFIX}";

        _player1PiecesValueDisplayText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(_player1Data.PieceColor)}>{_player1Data.AvailablePieces.Count}</color>";
        _player2PiecesValueDisplayText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(_player2Data.PieceColor)}>{_player2Data.AvailablePieces.Count}</color>";

        if (stateMessage.CurrentStateType.Equals(GameStateType.Removing))
        {
            //TODO play animation on mill text
            _millDisplayText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(stateMessage.CurrentPlayer.PieceColor)}>{MILL_TEXT}</color>";
            _millDisplayText.enabled = true;
        }
        else
            _millDisplayText.enabled = false;
    }

    public void OnMillCreated(GameEventMessage gameEventMessage)
    {
        StartCoroutine(TypeMessage(_millDisplayText, MILL_TEXT));
    }

    public void OnGameComplete(GameEventMessage gameEventMessage)
    {
        GameCompleteMessage completeMessage = (GameCompleteMessage)gameEventMessage;
        if (completeMessage == null)
            return;

        //palyer wins
        if (completeMessage.WinnerPlayer != null)
        {
            _gameOverNotificationText.text = $"Congrats, {completeMessage.WinnerPlayer}!! You win!\nBetter luck next time, {completeMessage.LoserPlayer}";
        }
        //draw
        else
        {
            _gameOverNotificationText.text = $"Draw! An even game indeed!\nHow about a rematch?";
        }

        _pausePanelAnimator.gameObject.SetActive(false);
        _gameOverPanelAnimator.Play("PanelPopupAnimOn");

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeMessage(_notificationDisplayText, "Game Over"));
    }

    public void OnGameLoaded()
    {
        var pos1 = Camera.main.ScreenToWorldPoint(_player1PiecesValueDisplayText.rectTransform.position);
        var pos2 = Camera.main.ScreenToWorldPoint(_player2PiecesValueDisplayText.rectTransform.position);

        pos1.z = 0;
        pos2.z = 0;

        _player1Data.WriteStashWorldPosition(pos1);
        _player2Data.WriteStashWorldPosition(pos2);
    }

    public void OnTogglePause(bool isPaused)
    {
        if (!isPaused && _audioPanel.IsEnabled)
            OnToggleAudioSettingsPanel(false);

        _gameBackground.raycastTarget = isPaused;

        _pausePanelAnimator.Play(isPaused ? "PanelSlideAnimOn" : "PanelSlideAnimOff");
    }

    public void OnToggleAudioSettingsPanel(bool isEnabled)
    {
        _pausePanel.interactable = !isEnabled;
        _audioPanel.ToggleEnable();
        _audioPanelAnimator.Play(isEnabled ? "PanelPopupAnimOn" : "PanelPopupAnimOff");
    }

    public void ButtonClickSFX()
    {
        _emitter.Emit(new AudioMessage("OnSFXRequested", AudioType.SFX, _audioData.ButtonClickSFX, false, null));
    }

    private IEnumerator TypeMessage(TextMeshProUGUI textUI, string message, float notificationLifetime = 0f)
    {
        textUI.text = "";

        float timeDelay = 1 / _gameData.NotificationTypeSpeed;

        WaitForSeconds typingDelay = new(timeDelay);
        textUI.maxVisibleCharacters = 0;
        textUI.text = message;

        for (int i = 0; i < textUI.text.Length; i++)
        {
            textUI.maxVisibleCharacters++;
            yield return typingDelay;
        }

        if (notificationLifetime > 0f)
        {
            yield return new WaitForSeconds(notificationLifetime);

            //textUI.text = "";
            textUI.maxVisibleCharacters = 0;
            textUI.text = _cachedNotification;

            for (int i = 0; i < textUI.text.Length; i++)
            {
                textUI.maxVisibleCharacters++;
                yield return typingDelay;
            }
        }

        _typingCoroutine = null;
    }
}