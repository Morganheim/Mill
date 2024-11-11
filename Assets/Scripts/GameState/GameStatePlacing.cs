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
        string message = $"{_gameManager.CurrentPlayer}'s turn. Place a piece on the board.";
        _gameManager.DisplayNotification(message);
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
            nodeMessage.Node.PlacePiece(_gameManager.CurrentPlayer.GetPiece());

            //TODO placing piece animation and effects

            //check for mill
            if (_gameManager.IsMill(nodeMessage.Node))
            {
                _gameManager.ChangeState(GameStateType.Removing);
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
            string message = $"Cannot place piece on already occupied spot! Choose an empty spot on the board!";
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
