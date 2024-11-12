using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateGameComplete : BaseGameState
{
    public GameStateGameComplete(GameManager gameManager, GameData gameData) : base(gameManager, gameData, GameStateType.GameComplete)
    {

    }

    public override void OnStateEnter()
    {
        //evaluate win draw, display appropriate message
        string message = $"Congrats, {_gameManager.WinnerPlayer}!! You win!";
        _gameManager.DisplayNotification(message);
    }

    public override void OnStateExit()
    {

    }

    public override void ProcessNodeClick(NodeMessage nodeMessage)
    {

    }

    public override void SwitchState()
    {

    }
}
