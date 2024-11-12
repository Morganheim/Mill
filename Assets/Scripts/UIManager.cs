using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerData _player1Data;
    [SerializeField] private PlayerData _player2Data;
    [SerializeField] private GameData _gameData;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _notificationDisplayText;
    [SerializeField] private TextMeshProUGUI _playerTurnDisplayText;
    [SerializeField] private TextMeshProUGUI _playerPiecesDisplayText;
    [SerializeField] private TextMeshProUGUI _millDisplayText;
    [SerializeField] private TextMeshProUGUI _gameStateDisplayText;

    private const string TURN_PREFIX = "Playing: ";
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

        _cachedNotification = _notificationDisplayText.text;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeMessage(_notificationDisplayText, notificationMessage.Message, notificationMessage.NotificationLifetime));
    }

    public void OnGameStateChanged(GameEventMessage gameEventMessage)
    {
        GameStateMessage stateMessage = (GameStateMessage)gameEventMessage;
        if (stateMessage == null)
            return;

        _playerTurnDisplayText.text = TURN_PREFIX + stateMessage.CurrentPlayer.ToString();
        _gameStateDisplayText.text = STATE_PREFIX + stateMessage.CurrentStateType.ToString();

        if (stateMessage.CurrentPlayer.AvailablePieces.Count > 0)
            _playerPiecesDisplayText.text = $"{PIECES_PREFIX}<color=#{ColorUtility.ToHtmlStringRGB(stateMessage.CurrentPlayer.PieceColor)}>{stateMessage.CurrentPlayer.AvailablePieces.Count}</color>";
        else
            _playerPiecesDisplayText.text = "";

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

    private IEnumerator TypeMessage(TextMeshProUGUI textUI, string message, float notificationLifetime = 0f)
    {
        textUI.text = "";

        float timeDelay = 1 / _gameData.NotificationTypeSpeed;

        WaitForSeconds typingDelay = new(timeDelay);

        var chars = message.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            textUI.text += chars[i];
            yield return typingDelay;
        }

        if (notificationLifetime > 0f)
        {
            yield return new WaitForSeconds(notificationLifetime);

            textUI.text = "";

            chars = _cachedNotification.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                textUI.text += chars[i];
                yield return typingDelay;
            }
        }

        _typingCoroutine = null;
    }
}
