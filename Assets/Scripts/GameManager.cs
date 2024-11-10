using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /**************************************** INSPECTOR VARIABLES ****************************************/
    [Header("Data")]
    [SerializeField] private GameData _gameData;

    [Header("Prefabs")]
    [SerializeField] private BoardNode _nodePrefab;
    [SerializeField] private GameObject _ringPrefab;
    [SerializeField] private BoardLine _linePrefab;

    [Header("Internal Components")]
    [SerializeField] private GameEventEmitter _emitter;

    /**************************************** PRIVATE VARIABLES ****************************************/
    private Dictionary<GameStateType, BaseGameState> _gameStates = new();
    private BaseGameState _currentState;
    private Player _currentPlayer;
    private List<BoardNode> _boardNodes = new();
    private Dictionary<Vector2Int, Node> _nodes = new();
    private List<LineRenderer> _lines = new();

    /**************************************** PROPERTIES ****************************************/
    public Player CurrentPlayer { get => _currentPlayer; }

    /**************************************** UNITY CALLBACKS ****************************************/
    private void OnEnable()
    {
        SetupBoard();
    }

    /**************************************** EVENT CALLBACKS ****************************************/
    public void OnNodeClicked(GameEventMessage gameEventMessage)
    {
        NodeMessage nodeMessage = (NodeMessage)gameEventMessage;
        if (nodeMessage == null)
            return;

        Debug.Log($"Clicked on node {nodeMessage.Node.BoardCoordinate}");

        //_currentState.ProcessNodeClick(nodeMessage);
    }

    /**************************************** PUBLIC METHODS ****************************************/
    public void ProcessTurnComplete()
    {
        //check if game state should change

        //if yes, change state

        //else switch current player, update prompt
    }

    /**************************************** PRIVATE METHODS ****************************************/
    private void GameInit()
    {
        _gameStates.Clear();

        _gameStates = new Dictionary<GameStateType, BaseGameState>
        {
            {GameStateType.Placing, new GameStatePlacing(this, _gameData) },
            {GameStateType.Moving, new GameStateMoving(this, _gameData) },
            {GameStateType.Removing, new GameStateRemoving(this, _gameData) },
        };

        _currentState = _gameStates[GameStateType.Placing];
        _currentState.OnStateEnter();
    }

    private void SetupBoard()
    {
        _nodes.Clear();
        _boardNodes.Clear();

        for (int ringIndex = 1; ringIndex <= _gameData.RingsAmount; ringIndex++)
        {
            var ring = Instantiate(_ringPrefab, Vector3.zero, Quaternion.identity, transform);
            ring.name = $"Ring_{ringIndex}";

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //center node
                    if (i == 0 && j == 0)
                        continue;

                    Vector2Int boardCoordinate = new(i * _gameData.PositionOffset * ringIndex, j * _gameData.PositionOffset * ringIndex);
                    Vector3 worldPosition = ring.transform.position + new Vector3(boardCoordinate.x, boardCoordinate.y, 0);

                    BoardNode boardNode = Instantiate(_nodePrefab, worldPosition, Quaternion.identity, ring.transform);
                    boardNode.gameObject.name = $"Node_{boardCoordinate.x}_{boardCoordinate.y}";

                    Node node = new(boardCoordinate, boardNode, ringIndex);
                    boardNode.SetupBoardNode(node);

                    _nodes.Add(boardCoordinate, node);
                    _boardNodes.Add(boardNode);

                    //testing
                    boardNode.GetComponent<SpriteRenderer>().color = ringIndex == 1 ? Color.red : (ringIndex == 2 ? Color.green : Color.blue);
                }
            }
        }

        _emitter.Emit("OnGameSetupComplete");
    }

    private void DrawLines()
    {
        _lines.Clear();

        foreach (var node in _nodes)
        {

        }
    }
}
