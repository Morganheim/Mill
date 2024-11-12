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
        string message;

        if (_gameManager.SelectedPiece == null)
            message = $"{_gameManager.CurrentPlayer}, pick one of your pieces to move";
        else
            message = $"{_gameManager.CurrentPlayer}, place the selected piece on one of the available spots";

        _gameManager.DisplayNotification(message);
    }

    public override void OnStateExit()
    {

    }

    public override void ProcessNodeClick(NodeMessage nodeMessage)
    {
        //null check
        if (!nodeMessage.Node.IsOccupied() && _gameManager.SelectedPiece == null)
            return;

        //check if the selected piece can move to the selected position
        if (!CanMovePieceToNode(nodeMessage.Node))
        {
            string message = $"You cannot place your piece on an occupied spot!";
            _gameManager.DisplayNotification(message);
            return;
        }

        //if node is empty, move piece to node
        if (!nodeMessage.Node.IsOccupied())
        {
            _gameManager.SelectedPiece.Node.RemovePiece();
            nodeMessage.Node.PlacePiece(_gameManager.SelectedPiece);

            //check for mills
            if (_gameManager.IsMill(nodeMessage.Node))
            {
                _gameManager.ChangeState(GameStateType.Removing);
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
        _gameManager.SelectedPiece = null;

        //check game complete
        if (!_gameManager.IsGameComplete())
        {
            _gameManager.SwitchPlayerTurn();
            _gameManager.ChangeState(StateType);
        }
        else
        {
            _gameManager.ChangeState(GameStateType.GameComplete);
        }
    }

    private bool CanMovePieceToNode(Node node)
    {
        return !node.IsOccupied() && (node.IsNeighbor(_gameManager.SelectedPiece.Node) || _gameData.EnablePiecesAlwaysFly);
    }
}
