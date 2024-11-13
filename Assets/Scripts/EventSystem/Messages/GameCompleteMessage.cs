using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCompleteMessage : GameEventMessage
{
    public PlayerData WinnerPlayer { get; private set; }
    public PlayerData LoserPlayer { get; private set; }

    public GameCompleteMessage(PlayerData winnerPlayer, PlayerData loserPlayer)
    {
        WinnerPlayer = winnerPlayer;
        LoserPlayer = loserPlayer;
    }

    public GameCompleteMessage(string eventName, PlayerData winnerPlayer, PlayerData loserPlayer) : base(eventName)
    {
        WinnerPlayer = winnerPlayer;
        LoserPlayer = loserPlayer;
    }
}
