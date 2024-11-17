using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData _gameData;
    [SerializeField] private AudioData _audioData;
    [SerializeField] private PlayerData _player1;
    [SerializeField] private PlayerData _player2;

    [Header("Internal Components")]
    [SerializeField] private GameBoardManager _boardManager;
    [SerializeField] private GameEventEmitter _emitter;

    private Dictionary<GameStateType, BaseGameState> _gameStates = new();
    private BaseGameState _currentState;

    public PlayerData CurrentPlayer { get; private set; }
    public PlayerData OpponentPlayer => CurrentPlayer == _player1 ? _player2 : _player1;
    public Node HoveredNode { get; private set; }
    public PlayerPiece SelectedPiece { get; set; }
    public AudioData AudioData => _audioData;

    public void OnNodeClicked(GameEventMessage gameEventMessage)
    {
        NodeMessage nodeMessage = (NodeMessage)gameEventMessage;
        if (nodeMessage == null)
            return;

        _currentState.ProcessNodeClick(nodeMessage.Node);
    }

    public void OnNodeHoverEnter(GameEventMessage gameEventMessage)
    {
        NodeMessage nodeMessage = (NodeMessage)gameEventMessage;
        if (nodeMessage == null)
            return;

        HoveredNode = nodeMessage.Node;

        _currentState.ProcessNodeHover(true, nodeMessage.Node);
    }

    public void OnNodeHoverExit(GameEventMessage gameEventMessage)
    {
        NodeMessage nodeMessage = (NodeMessage)gameEventMessage;
        if (nodeMessage == null)
            return;

        HoveredNode = null;

        _currentState.ProcessNodeHover(false, nodeMessage.Node);
    }

    public void OnDeselectPiece()
    {
        if (SelectedPiece == null)
            return;

        if (HoveredNode != null && !HoveredNode.IsOccupied())
            HoveredNode.BoardNode.ToggleHighlight(false);

        SelectedPiece.Node.BoardNode.ToggleHighlight(false);
        SelectedPiece = null;
        _currentState.OnStateEnter();
    }

    public void OnGameLoaded()
    {
        GameInit();
    }

    public void ChangeState(GameStateType stateType)
    {
        GameStateType previousStateType = _currentState.StateType;

        _currentState.OnStateExit();
        _currentState = _gameStates[stateType];
        _currentState.OnStateEnter();

        _emitter.Emit(new GameStateMessage("OnGameStateChange", previousStateType, stateType, CurrentPlayer, OpponentPlayer));
    }

    public void SwitchPlayerTurn()
    {
        CurrentPlayer = OpponentPlayer;
    }

    public void DisplayNotification(string message)
    {
        _emitter.Emit(new NotificationMessage("OnDisplayNotificationRequested", message));
    }

    public void DisplayTempNotification(string message)
    {
        _emitter.Emit(new NotificationMessage("OnTempNotificationRequested", message, _gameData.NotificationLifetime));
    }

    public void RequestSFX(AudioClip clip, bool isLooping, UnityEvent onStopLoop, AudioType audioType = AudioType.SFX)
    {
        _emitter.Emit(new AudioMessage("OnSFXRequested", audioType, clip, isLooping, onStopLoop));
    }

    public void SelectPiece(PlayerPiece piece)
    {
        SelectedPiece = piece;
        SelectedPiece.Node.BoardNode.ToggleHighlight(true, CurrentPlayer.PieceColor);
        _currentState.OnStateEnter();
    }

    public bool IsGameComplete()
    {
        if (IsLoseCondition() || IsDrawCondition())
        {
            //cache winner if there is a winner
            PlayerData winner = IsDrawCondition() ? null : CurrentPlayer;
            PlayerData loser = IsDrawCondition() ? null : OpponentPlayer;

            RequestSFX(winner != null ? _audioData.WinSFX : _audioData.DrawSFX, false, null, AudioType.Music);

            //display game over screen
            _emitter.Emit(new GameCompleteMessage("OnGameComplete", winner, loser));

            return true;
        }

        return false;
    }

    public bool IsMill(Node node)
    {
        var createdMills = _boardManager.GetNodeMills(node);
        bool flag = false;

        foreach (var mill in createdMills)
            if (!CurrentPlayer.IsMillDuplicate(mill))
                flag = true;

        if (flag)
            RequestSFX(_audioData.MillSFX, false, null);

        return flag;
    }

    public bool CanMovePieceToNode(PlayerPiece piece, Node node)
    {
        return !node.IsOccupied() && (node.IsNeighbor(piece.Node) || _gameData.IsPiecesFlyEnabled);
    }

    public bool IsNodeMovable(Node node)
    {
        if (!node.IsOccupied())
            return false;

        if (_gameData.IsPiecesFlyEnabled)
            return true;

        HashSet<Node> neighbors = _boardManager.GetNodeNeighbors(node);

        foreach (var neighbor in neighbors)
            if (!neighbor.IsOccupied())
                return true;

        return false;
    }

    private void GameInit()
    {
        _player1.InitPlayer(_gameData.PiecesInitialAmount);
        _player2.InitPlayer(_gameData.PiecesInitialAmount);

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

        _emitter.Emit(new GameStateMessage("OnGameStateChange", GameStateType.None, _currentState.StateType, CurrentPlayer, OpponentPlayer));
    }

    private bool IsLoseCondition()
    {
        bool flag = true;

        //2 pieces left on board and no more to place
        //no legal moves
        if (!OpponentPlayer.IsLoser())
        {
            foreach (var piece in OpponentPlayer.PlacedPieces)
            {
                if (IsNodeMovable(piece.Node))
                {
                    flag = false;
                    break;
                }
            }
        }

        return flag;
    }

    private bool IsDrawCondition()
    {
        bool flag = true;

        //no legal moves for both players
        foreach (var piece in CurrentPlayer.PlacedPieces)
        {
            if (IsNodeMovable(piece.Node))
            {
                flag = false;
                break;
            }
        }

        if (flag)
        {
            foreach (var piece in OpponentPlayer.PlacedPieces)
            {
                if (IsNodeMovable(piece.Node))
                {
                    flag = false;
                    break;
                }
            }
        }

        return flag;
    }
}