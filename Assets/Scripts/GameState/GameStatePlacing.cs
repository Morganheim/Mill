using UnityEngine;

public class GameStatePlacing : BaseGameState
{
    public GameStatePlacing(GameStateManager gameStateManager, GameData gameData) : base(gameStateManager, gameData, GameStateType.Placing)
    {

    }

    public override void OnStateEnter()
    {
        b_Message = $"Place a piece on the board.";
        b_GameStateManager.DisplayNotification(b_Message);
    }

    public override void OnStateExit()
    {

    }

    public override void ProcessNodeClick(Node node)
    {
        //place piece
        if (!node.IsOccupied())
        {
            //occupy node
            node.PlacePiece(b_GameStateManager.CurrentPlayer.GetPiece());

            node.BoardNode.ToggleHighlight(false);

            //play sfx
            b_GameStateManager.RequestSFX(b_GameStateManager.AudioData.PiecePlacementSFX, false, null);

            //check for mill
            if (b_GameStateManager.IsMill(node))
            {
                b_GameStateManager.ChangeState(GameStateType.Removing);
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
            b_Message = $"Cannot place the piece on an already occupied position!\nChoose an empty position on the board!";
            b_GameStateManager.DisplayTempNotification(b_Message);
        }

    }

    public override void ProcessNodeHover(bool isEnter, Node node)
    {
        bool toggle = false;
        Color color = node.BoardNode.SpriteRenderer.color;

        //hover enter
        if (isEnter)
        {
            if (!node.IsOccupied())
            {
                toggle = true;
                color = b_GameStateManager.CurrentPlayer.PieceColor;
            }

            if (toggle)
                node.BoardNode.ToggleHighlight(toggle, color);
        }

        //hover exit
        else
        {
            node.BoardNode.ToggleHighlight(false);
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
