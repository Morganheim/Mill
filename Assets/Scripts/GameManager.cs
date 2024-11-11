using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /**************************************** INSPECTOR VARIABLES ****************************************/
    [Header("Debug")]
    public GameStateType currentStateType;


    [Header("Data")]
    [SerializeField] private GameData _gameData;
    [SerializeField] private PlayerData _player1;
    [SerializeField] private PlayerData _player2;

    [Header("Internal Components")]
    [SerializeField] private GameBoardManager _boardManager;
    [SerializeField] private GameEventEmitter _emitter;

    /**************************************** PRIVATE VARIABLES ****************************************/
    private Dictionary<GameStateType, BaseGameState> _gameStates = new();
    private BaseGameState _currentState;

    /**************************************** PROPERTIES ****************************************/
    public PlayerData CurrentPlayer { get; private set; }
    public PlayerData OpponentPlayer { get => CurrentPlayer == _player1 ? _player2 : _player1; }
    public PlayerPiece SelectedPiece { get; set; }

    /**************************************** UNITY CALLBACKS ****************************************/
    private void OnEnable()
    {
        //_emitter.Emit("OnGameSetupComplete");
        GameInit();
    }

    /**************************************** EVENT CALLBACKS ****************************************/
    public void OnNodeClicked(GameEventMessage gameEventMessage)
    {
        NodeMessage nodeMessage = (NodeMessage)gameEventMessage;
        if (nodeMessage == null)
            return;

        if (nodeMessage.Node.IsOccupied() && _currentState.StateType.Equals(GameStateType.Moving) && SelectedPiece == null)
        {
            if (nodeMessage.Node.Piece.Owner != CurrentPlayer)
            {
                DisplayNotification($"No stealing, {CurrentPlayer}!");
                return;
            }

            //select piece
            if (IsNodeMovable(nodeMessage.Node))
            {
                SelectedPiece = nodeMessage.Node.Piece;
                _currentState.OnStateEnter();
                return;
            }
            //node has no legal moves, pick different node message
            else
            {
                DisplayNotification($"Selected piece can't be moved, pick a different piece!");
                return;
            }
        }

        _currentState.ProcessNodeClick(nodeMessage);
    }

    /**************************************** PUBLIC METHODS ****************************************/
    public void ChangeState(GameStateType stateType)
    {
        _currentState.OnStateExit();
        _currentState = _gameStates[stateType];
        _currentState.OnStateEnter();
        currentStateType = stateType;
    }

    public void SwitchPlayerTurn()
    {
        CurrentPlayer = OpponentPlayer;
    }

    public void CheckGameComplete()
    {
        //lose conditions
        //2 pieces left on board and no more to place
        //no legal moves

        //draw conditions
        //no legal moves for both players
    }

    public void DisplayNotification(string message)
    {
        _emitter.Emit(new StringMessage("OnDisplayNotificationRequested", message));
    }

    public bool IsMill(Node node)
    {
        var createdMills = _boardManager.GetNodeMills(node);
        bool flag = false;

        foreach (var mill in createdMills)
            if (!CurrentPlayer.IsMillDuplicate(mill))
                flag = true;

        return flag;
    }

    /**************************************** PRIVATE METHODS ****************************************/
    private void GameInit()
    {
        _player1.InitPlayer();
        _player2.InitPlayer();

        CurrentPlayer = _player1;

        SelectedPiece = null;

        _gameStates.Clear();

        _gameStates = new Dictionary<GameStateType, BaseGameState>
        {
            {GameStateType.Placing, new GameStatePlacing(this, _gameData) },
            {GameStateType.Moving, new GameStateMoving(this, _gameData) },
            {GameStateType.Removing, new GameStateRemoving(this, _gameData) },
        };

        _currentState = _gameStates[GameStateType.Placing];
        _currentState.OnStateEnter();
        currentStateType = GameStateType.Placing;
    }

    private bool IsNodeMovable(Node node)
    {
        HashSet<Node> neighbors = _boardManager.GetNodeNeighbors(node);

        foreach (var neighbor in neighbors)
            if (!neighbor.IsOccupied())
                return true;

        return false;
    }
}
