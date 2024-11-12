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

        b_Message = $"Congrats, {b_GameManager.WinnerPlayer}!! You < color =#{ColorUtility.ToHtmlStringRGB(b_GameManager.CurrentPlayer.PieceColor)}>WIN!</color>!";
        b_GameManager.DisplayNotification(b_Message);
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
