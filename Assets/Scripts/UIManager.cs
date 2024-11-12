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
    [SerializeField] private TextMeshProUGUI _gameStateDisplayText;

    private const string TURN_PREFIX = "Playing: ";
    private const string STATE_PREFIX = "Phase: ";

    private string _cachedNotification;

    public void OnDisplayNotification(GameEventMessage gameEventMessage)
    {
        StringMessage stringMessage = (StringMessage)gameEventMessage;
        if (stringMessage == null)
            return;

        _cachedNotification = stringMessage.StringValue;
        _notificationDisplayText.text = stringMessage.StringValue;
    }

    public void OnDisplayTempNotification(GameEventMessage gameEventMessage)
    {
        StringMessage stringMessage = (StringMessage)gameEventMessage;
        if (stringMessage == null)
            return;

        _cachedNotification = _notificationDisplayText.text;
    }

    public void OnGameStateChanged(GameEventMessage gameEventMessage)
    {
        GameStateMessage stateMessage = (GameStateMessage)gameEventMessage;
        if (stateMessage == null)
            return;

        _playerTurnDisplayText.text = TURN_PREFIX + stateMessage.CurrentPlayer.ToString();
        _gameStateDisplayText.text = STATE_PREFIX + stateMessage.CurrentStateType.ToString();
    }

    private IEnumerator TypeMessage(string message, float notificationLifetime = 0f)
    {
        _notificationDisplayText.text = "";
        var chars = message.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            _notificationDisplayText.text += chars[i];
            yield return new WaitForSeconds(100 / _gameData.NotificationTypeSpeed);
        }

        yield return new WaitForSeconds(notificationLifetime);

        chars = _cachedNotification.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            _notificationDisplayText.text += chars[i];
            yield return new WaitForSeconds(100 / _gameData.NotificationTypeSpeed);
        }
    }
}
