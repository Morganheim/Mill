using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerData _player1Data;
    [SerializeField] private PlayerData _player2Data;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _notificationDisplayText;

    public void OnDisplayNotification(GameEventMessage gameEventMessage)
    {
        StringMessage stringMessage = (StringMessage)gameEventMessage;
        if (stringMessage == null)
            return;

        _notificationDisplayText.text = stringMessage.StringValue;
    }
}
