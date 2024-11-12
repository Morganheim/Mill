using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMessage : GameEventMessage
{
    public GameStateType PreviousStateType { get; private set; }
    public GameStateType CurrentStateType { get; private set; }
    public PlayerData CurrentPlayer { get; private set; }

    public GameStateMessage(GameStateType previousStateType, GameStateType currentStateType, PlayerData currentPlayer)
    {
        PreviousStateType = previousStateType;
        CurrentStateType = currentStateType;
        CurrentPlayer = currentPlayer;
    }

    public GameStateMessage(string eventName, GameStateType previousStateType, GameStateType currentStateType, PlayerData currentPlayer) : base(eventName)
    {
        PreviousStateType = previousStateType;
        CurrentStateType = currentStateType;
        CurrentPlayer = currentPlayer;
    }
}
