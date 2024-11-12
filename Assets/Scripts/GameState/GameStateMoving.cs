using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMoving : BaseGameState
{
    public GameStateMoving(GameManager gameManager, GameData gameData) : base(gameManager, gameData, GameStateType.Moving)
    {

    }

    public override void OnStateEnter()
    {
        if (b_GameManager.SelectedPiece == null)
            b_Message = $"Pick one of your own pieces to move.";
        else
            b_Message = $"Place the selected piece on one of the available spots.";

        b_GameManager.DisplayNotification(b_Message);
    }

    public override void OnStateExit()
    {

    }

    public override void ProcessNodeClick(NodeMessage nodeMessage)
    {
        //null check
        if (!nodeMessage.Node.IsOccupied() && b_GameManager.SelectedPiece == null)
        {
            b_Message = $"That spot is empty!\nPick one of your own pieces to move!";
            b_GameManager.DisplayTempNotification(b_Message);
            return;
        }

        //check if the selected piece can move to the selected position
        if (!b_GameManager.CanMovePieceToNode(b_GameManager.SelectedPiece, nodeMessage.Node))
        {
            b_Message = $"You cannot place the piece on an already occupied spot!";
            b_GameManager.DisplayTempNotification(b_Message);
            return;
        }

        //if node is empty, move piece to node
        if (!nodeMessage.Node.IsOccupied())
        {
            b_GameManager.SelectedPiece.Node.RemovePiece();
            nodeMessage.Node.PlacePiece(b_GameManager.SelectedPiece);

            //check for mills
            if (b_GameManager.IsMill(nodeMessage.Node))
            {
                b_GameManager.ChangeState(GameStateType.Removing);
                return;
            }
            else
            {
                SwitchState();
            }
        }

        //else show error message
    }

    public override void SwitchState()
    {
        b_GameManager.SelectedPiece = null;

        //check game complete
        if (!b_GameManager.IsGameComplete())
        {
            b_GameManager.SwitchPlayerTurn();
            b_GameManager.ChangeState(StateType);
        }
        else
        {
            b_GameManager.ChangeState(GameStateType.GameComplete);
        }
    }
}
