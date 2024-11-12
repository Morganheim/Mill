using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlacing : BaseGameState
{
    public GameStatePlacing(GameManager gameManager, GameData gameData) : base(gameManager, gameData, GameStateType.Placing)
    {

    }

    public override void OnStateEnter()
    {
        b_Message = $"Place a piece on the board.";
        b_GameManager.DisplayNotification(b_Message);
    }

    public override void OnStateExit()
    {

    }

    public override void ProcessNodeClick(NodeMessage nodeMessage)
    {
        //place piece
        if (!nodeMessage.Node.IsOccupied())
        {
            //occupy node
            nodeMessage.Node.PlacePiece(b_GameManager.CurrentPlayer.GetPiece());

            //TODO placing piece animation and effects

            //check for mill
            if (b_GameManager.IsMill(nodeMessage.Node))
            {
                b_GameManager.ChangeState(GameStateType.Removing);
            }
            //no mill
            else
            {
                //complete turn
                SwitchState();
            }
        }
        //show error message
        else
        {
            b_Message = $"Cannot place the piece on an already occupied spot!\nChoose an empty spot on the board!";
            b_GameManager.DisplayTempNotification(b_Message);
        }

    }

    public override void SwitchState()
    {
        if (b_GameManager.OpponentPlayer.AvailablePieces.Count > 0 || b_GameManager.CurrentPlayer.AvailablePieces.Count > 0)
        {
            //check next player
            if (b_GameManager.OpponentPlayer.AvailablePieces.Count > 0)
                b_GameManager.SwitchPlayerTurn();

            //display player turn message
            b_GameManager.ChangeState(GameStateType.Placing);
        }
        else
        {
            //switch player
            b_GameManager.SwitchPlayerTurn();

            //change game state to moving
            b_GameManager.ChangeState(GameStateType.Moving);
        }
    }
}
