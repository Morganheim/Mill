using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateRemoving : BaseGameState
{
    public GameStateRemoving(GameManager gameManager, GameData gameData) : base(gameManager, gameData, GameStateType.Removing)
    {

    }

    public override void OnStateEnter()
    {

        b_Message = $"Pick an opponent piece to destroy!";
        b_GameManager.DisplayNotification(b_Message);
    }

    public override void OnStateExit()
    {
        b_GameManager.SelectedPiece = null;
    }

    public override void ProcessNodeClick(NodeMessage nodeMessage)
    {
        if (nodeMessage.Node.IsOccupied())
        {
            //if node is occupied by opponent && is not in mill, remove piece
            if (nodeMessage.Node.Piece.Owner != b_GameManager.CurrentPlayer)
            {
                if (b_GameManager.OpponentPlayer.IsNodePartOfAnActiveMill(nodeMessage.Node) && b_GameManager.OpponentPlayer.IsThereAPlacedNonMillPiece())
                {
                    b_Message = $"Cannot destroy that piece, it's part of a mill!";
                    b_GameManager.DisplayTempNotification(b_Message);
                }
                else
                {
                    //destroy piece
                    nodeMessage.Node.RemovePiece(true);

                    //check game complete
                    if (!b_GameManager.IsGameComplete())
                        SwitchState();
                    else
                        b_GameManager.ChangeState(GameStateType.GameComplete);

                }
            }
            //else if node is occupied by player, show error message "cannot remove your own pieces"
            else
            {
                b_Message = $"Destroy a <color=#{ColorUtility.ToHtmlStringRGB(b_GameManager.OpponentPlayer.PieceColor)}>{b_GameManager.OpponentPlayer.PieceColor}</color> piece! Not your own!";
                b_GameManager.DisplayTempNotification(b_Message);
            }
        }
        //else, show error message for empty node
        else
        {
            b_Message = $"That spot is empty!\nPick a spot holding your opponent's piece!";
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
