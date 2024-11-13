using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMoving : BaseGameState
{
    public GameStateMoving(GameStateManager gameStateManager, GameData gameData) : base(gameStateManager, gameData, GameStateType.Moving)
    {

    }

    public override void OnStateEnter()
    {
        if (b_GameStateManager.SelectedPiece == null)
            b_Message = $"Pick one of your own pieces to move.";
        else
            b_Message = $"Place the selected piece on one of the available spots.";

        b_GameStateManager.DisplayNotification(b_Message);

        if (b_GameStateManager.SelectedPiece == null && b_GameStateManager.HoveredNode != null && b_GameStateManager.IsNodeMovable(b_GameStateManager.HoveredNode) && b_GameStateManager.HoveredNode.Piece.Owner == b_GameStateManager.CurrentPlayer)
        {
            //Color color = b_GameManager.HoveredNode.BoardNode.SpriteRenderer.color;
            Color color = b_GameStateManager.CurrentPlayer.PieceColor;
            b_GameStateManager.HoveredNode.BoardNode.ToggleHighlight(true, new Color(color.r, color.g, color.b, b_GameData.OwnedNodeHighlightTransparency));
        }
    }

    public override void OnStateExit()
    {
    }

    public override void ProcessNodeClick(Node node)
    {
        //player has not selected piece to move
        if(b_GameStateManager.SelectedPiece == null)
        {
            //selected empty node instead of picking own piece
            if (!node.IsOccupied())
            {
                b_Message = $"That position is empty!\nPick one of your own pieces to move!";
                b_GameStateManager.DisplayTempNotification(b_Message);
                return;
            }
            //attempted to select opponent's piece
            if (node.Piece.Owner == b_GameStateManager.OpponentPlayer)
            {
                b_Message = $"No stealing, {b_GameStateManager.CurrentPlayer}!";
                b_GameStateManager.DisplayTempNotification(b_Message);
                return;
            }
            //attempted to select piece that has no legal moves
            if (!b_GameStateManager.IsNodeMovable(node))
            {
                b_Message = $"Selected piece can't be moved!\nPick a different piece!";
                b_GameStateManager.DisplayTempNotification(b_Message);
                return;
            }

            b_GameStateManager.SelectPiece(node.Piece);
        }

        //check if the selected piece can move to the selected node
        else
        {
            //selected node is occupied
            if (node.IsOccupied())
            {
                //if clicked on an owned and movable piece, change selected piece to new piece
                if (node.Piece.Owner == b_GameStateManager.CurrentPlayer && b_GameStateManager.IsNodeMovable(node))
                {
                    b_GameStateManager.SelectedPiece.Node.BoardNode.ToggleHighlight(false);
                    b_GameStateManager.SelectedPiece = node.Piece;
                    b_GameStateManager.SelectedPiece.Node.BoardNode.ToggleHighlight(true, b_GameStateManager.CurrentPlayer.PieceColor);
                    return;
                }

                b_Message = $"You can't place the piece on an already occupied position!";
                b_GameStateManager.DisplayTempNotification(b_Message);
                return;
            }
            //selected node is out of range for the selected piece
            if(!b_GameStateManager.CanMovePieceToNode(b_GameStateManager.SelectedPiece, node))
            {
                b_Message = $"Target position out of range of the selected piece!\nPick a different position!";
                b_GameStateManager.DisplayTempNotification(b_Message);
                return;
            }

            //move selected piece to selected node
            b_GameStateManager.SelectedPiece.Node.BoardNode.ToggleHighlight(false);
            b_GameStateManager.SelectedPiece.Node.RemovePiece();
            node.PlacePiece(b_GameStateManager.SelectedPiece);
            node.BoardNode.ToggleHighlight(false);

            //check for mills
            if (b_GameStateManager.IsMill(node))
            {
                b_GameStateManager.ChangeState(GameStateType.Removing);
                return;
            }
            else
            {
                SwitchState();
            }
        }
    }

    public override void ProcessNodeHover(bool isEnter, Node node)
    {
        bool toggle = false;
        Color color = node.BoardNode.SpriteRenderer.color;

        //hover enter
        if (isEnter)
        {
            //for selectiong piece
            if (b_GameStateManager.SelectedPiece == null)
            {
                //if node is empty && current player owns piece && node is movable
                if (node.IsOccupied() && node.Piece.Owner == b_GameStateManager.CurrentPlayer && b_GameStateManager.IsNodeMovable(node))
                {
                    toggle = true;
                    color = new Color(color.r, color.g, color.b, b_GameData.OwnedNodeHighlightTransparency);
                }
            }

            //if piece is selected
            else
            {
                //for selecting node to move to
                if (!node.IsOccupied() && b_GameStateManager.CanMovePieceToNode(b_GameStateManager.SelectedPiece, node))
                {
                    toggle = true;
                    color = b_GameStateManager.CurrentPlayer.PieceColor;
                }

                //for selecting different owned piece to move
                else if (node.IsOccupied() && node.Piece.Owner == b_GameStateManager.CurrentPlayer && b_GameStateManager.IsNodeMovable(node))
                {
                    toggle = true;
                    color = new Color(color.r, color.g, color.b, b_GameData.OwnedNodeHighlightTransparency);
                }
            }

            if (toggle)
                node.BoardNode.ToggleHighlight(toggle, color);
        }

        //hover exit
        else
        {
            if (node.Piece == b_GameStateManager.SelectedPiece)
                return;

            node.BoardNode.ToggleHighlight(false);
        }
    }

    public override void SwitchState()
    {
        b_GameStateManager.SelectedPiece.Node.BoardNode.ToggleHighlight(false);
        b_GameStateManager.SelectedPiece = null;

        //check game complete
        if (!b_GameStateManager.IsGameComplete())
        {
            b_GameStateManager.SwitchPlayerTurn();
            b_GameStateManager.ChangeState(StateType);
        }
    }
}
