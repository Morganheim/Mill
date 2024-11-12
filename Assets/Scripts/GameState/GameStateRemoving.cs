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
        string message = $"MILL! {_gameManager.CurrentPlayer}, destroy an opponent piece!";
        _gameManager.DisplayNotification(message);
    }

    public override void OnStateExit()
    {
        _gameManager.SelectedPiece = null;
    }

    public override void ProcessNodeClick(NodeMessage nodeMessage)
    {
        string message;

        if (nodeMessage.Node.IsOccupied())
        {
            //if node is occupied by opponent && is not in mill, remove piece
            if (nodeMessage.Node.Piece.Owner != _gameManager.CurrentPlayer)
            {
                if (_gameManager.OpponentPlayer.IsNodePartOfAnActiveMill(nodeMessage.Node) && _gameManager.OpponentPlayer.IsThereAPlacedNonMillPiece())
                {
                    message = $"Cannot destroy that piece, it's part of a mill!";
                    _gameManager.DisplayNotification(message);
                }
                else
                {
                    //destroy piece
                    nodeMessage.Node.RemovePiece(true);

                    //check game complete
                    if (!_gameManager.IsGameComplete())
                        SwitchState();
                    else
                        _gameManager.ChangeState(GameStateType.GameComplete);

                }
            }
            //else if node is occupied by player, show error message "cannot remove your own pieces"
            else
            {
                message = $"{_gameManager.CurrentPlayer}, destroy a <color=#{ColorUtility.ToHtmlStringRGB(_gameManager.OpponentPlayer.PieceColor)}>{_gameManager.OpponentPlayer.PieceColor}</color> piece! Not your own!";
                _gameManager.DisplayNotification(message);
            }
        }
        //else, show error message for empty node
        else
        {
            message = $"There's no piece on that spot!";
            _gameManager.DisplayNotification(message);
        }


    }

    public override void SwitchState()
    {
        if (_gameManager.OpponentPlayer.AvailablePieces.Count > 0 || _gameManager.CurrentPlayer.AvailablePieces.Count > 0)
        {
            //check next player
            if (_gameManager.OpponentPlayer.AvailablePieces.Count > 0)
                _gameManager.SwitchPlayerTurn();

            //display player turn message
            _gameManager.ChangeState(GameStateType.Placing);
        }
        else
        {
            //switch player
            _gameManager.SwitchPlayerTurn();

            //change game state to moving
            _gameManager.ChangeState(GameStateType.Moving);
        }
    }
}
