using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameState
{
    public BaseGameState(GameManager gameManager, GameData gameData, GameStateType gameStateType)
    {
        _gameManager = gameManager;
        _gameData = gameData;
        StateType = gameStateType;
    }

    protected GameManager _gameManager;
    protected GameData _gameData;

    public GameStateType StateType;

    public abstract void OnStateEnter();

    public abstract void OnStateExit();

    public abstract void ProcessNodeClick(NodeMessage nodeMessage);

    public abstract void SwitchState();
}

public enum GameStateType { None = 0, Placing = 1, Moving = 2, Removing = 3 }