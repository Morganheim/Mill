using UnityEngine;

public class GameStateRemoving : BaseGameState
{
    public GameStateRemoving(GameStateManager gameStateManager, GameData gameData) : base(gameStateManager, gameData, GameStateType.Removing)
    {

    }

    public override void OnStateEnter()
    {

        b_Message = $"Pick an opponent piece to destroy!";
        b_GameStateManager.DisplayNotification(b_Message);

        //highlight all non-mill opponent piece holding nodes or all opponent piece holding nodes if opponent only has nodes in mills
        foreach (var piece in b_GameStateManager.OpponentPlayer.PlacedPieces)
            if (!b_GameStateManager.OpponentPlayer.IsThereAPlacedNonMillPiece() || !b_GameStateManager.OpponentPlayer.IsNodePartOfAnActiveMill(piece.Node))
                piece.Node.BoardNode.ToggleHighlight(true, b_GameStateManager.CurrentPlayer.PieceColor);
    }

    public override void OnStateExit()
    {
        b_GameStateManager.SelectedPiece = null;
    }

    public override void ProcessNodeClick(Node node)
    {
        if (node.IsOccupied())
        {
            //if node is occupied by opponent && is not in mill, remove piece
            if (node.Piece.Owner != b_GameStateManager.CurrentPlayer)
            {
                if (b_GameStateManager.OpponentPlayer.IsNodePartOfAnActiveMill(node) && b_GameStateManager.OpponentPlayer.IsThereAPlacedNonMillPiece())
                {
                    b_Message = $"Cannot destroy that piece, it's part of a mill!";
                    b_GameStateManager.DisplayTempNotification(b_Message);
                }
                else
                {
                    //destroy piece
                    node.RemovePiece(true);

                    b_GameStateManager.RequestSFX(b_GameStateManager.AudioData.PieceRemoveSFX, false, null);

                    foreach (var piece in b_GameStateManager.OpponentPlayer.PlacedPieces)
                        piece.Node.BoardNode.ToggleHighlight(false);

                    node.BoardNode.ToggleHighlight(false);

                    //check game complete
                    if (!b_GameStateManager.IsGameComplete())
                        SwitchState();

                }
            }
            //else if node is occupied by player, show error message "cannot remove your own pieces"
            else
            {
                b_Message = $"Destroy <color=#{ColorUtility.ToHtmlStringRGB(b_GameStateManager.OpponentPlayer.PieceColor)}>{b_GameStateManager.OpponentPlayer}'s</color> piece! Not your own!";
                b_GameStateManager.DisplayTempNotification(b_Message);
            }
        }
        //else, show error message for empty node
        else
        {
            b_Message = $"That position is empty!\nPick a position holding your opponent's piece!";
            b_GameStateManager.DisplayTempNotification(b_Message);
        }
    }

    public override void ProcessNodeHover(bool isEnter, Node node)
    {
        bool toggle = false;
        Color color = Color.white;

        //hover enter
        if (isEnter)
        {
            toggle = false;
            color = new(node.BoardNode.SpriteRenderer.color.r, node.BoardNode.SpriteRenderer.color.g, node.BoardNode.SpriteRenderer.color.b);

            if (node.IsOccupied() && node.Piece.Owner == b_GameStateManager.OpponentPlayer && (!b_GameStateManager.OpponentPlayer.IsThereAPlacedNonMillPiece() || !b_GameStateManager.OpponentPlayer.IsNodePartOfAnActiveMill(node)))
            {
                toggle = true;
                color = Color.white;
            }

            if (toggle)
                node.BoardNode.ToggleHighlight(toggle, color);
        }

        //hover exit
        else
        {
            toggle = false;
            color = node.BoardNode.SpriteRenderer.color;

            if (node.IsOccupied() && node.Piece.Owner == b_GameStateManager.OpponentPlayer && (!b_GameStateManager.OpponentPlayer.IsThereAPlacedNonMillPiece() || !b_GameStateManager.OpponentPlayer.IsNodePartOfAnActiveMill(node)))
            {
                toggle = true;
                color = b_GameStateManager.CurrentPlayer.PieceColor;
            }

            node.BoardNode.ToggleHighlight(toggle, color);
        }
    }

    public override void SwitchState()
    {
        if (b_GameStateManager.OpponentPlayer.AvailablePieces.Count > 0 || b_GameStateManager.CurrentPlayer.AvailablePieces.Count > 0)
        {
            //check next player
            if (b_GameStateManager.OpponentPlayer.AvailablePieces.Count > 0)
                b_GameStateManager.SwitchPlayerTurn();

            //display player turn message
            b_GameStateManager.ChangeState(GameStateType.Placing);
        }
        else
        {
            //switch player
            b_GameStateManager.SwitchPlayerTurn();

            //change game state to moving
            b_GameStateManager.ChangeState(GameStateType.Moving);
        }
    }
}
