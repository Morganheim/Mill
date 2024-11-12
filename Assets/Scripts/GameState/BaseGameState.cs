using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameState
{
    public BaseGameState(GameManager gameManager, GameData gameData, GameStateType gameStateType)
    {
        b_GameManager = gameManager;
        b_GameData = gameData;
        StateType = gameStateType;
    }

    protected GameManager b_GameManager;
    protected GameData b_GameData;
    protected string b_Message;

    public GameStateType StateType { get; protected set; }

    public abstract void OnStateEnter();

    public abstract void OnStateExit();

    public abstract void ProcessNodeClick(NodeMessage nodeMessage);

    public abstract void SwitchState();
}

public enum GameStateType { None = 0, Placing = 1, Moving = 2, Removing = 3, GameComplete = 4 }