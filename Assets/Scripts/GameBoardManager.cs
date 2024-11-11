using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData _gameData;

    [Header("Prefabs")]
    [SerializeField] private BoardNode _nodePrefab;
    [SerializeField] private GameObject _ringPrefab;
    [SerializeField] private BoardLine _linePrefab;

    [Header("Internal Components")]
    [SerializeField] private GameEventEmitter _emitter;

    private readonly Dictionary<Vector2Int, Node> _nodes = new();
    private readonly List<BoardNode> _boardNodes = new();
    private readonly List<BoardLine> _lines = new();

    private void OnEnable()
    {
        SetupBoard();

        _emitter.Emit("OnGameBoardSetupComplete");
    }

    public Node GetNode(Vector2Int boardCoordinate)
    {
        return _nodes.ContainsKey(boardCoordinate) ? _nodes[boardCoordinate] : null;
    }

    public HashSet<Node> GetNodeNeighbors(Node node)
    {
        HashSet<Node> result = new();
        foreach (var coordinateDirection in node.ConnectionDirections)
        {
            Vector2Int key = node.BoardCoordinate + coordinateDirection;

            if (_nodes.ContainsKey(key))
                result.Add(_nodes[key]);
        }

        return result;
    }

    public List<HashSet<Node>> GetNodeMills(Node node)
    {
        List<HashSet<Node>> millsList = new();
        List<HashSet<Node>> uniqueMillsList = new();

        //foreach direction in node.direction
        foreach (var connectionDirection in node.ConnectionDirections)
        {
            HashSet<Node> mill = new() { node };

            //check if node in that position exists
            var nextNode = GetNode(node.BoardCoordinate + connectionDirection);
            if (nextNode == null)
                continue;

            //if yes, check if is occupied && piece on node belongs to same player as the first one
            if (IsNodeMillConditionSatisfied(node, nextNode))
            {
                //if yes, check same direction on that next node
                mill.Add(nextNode);

                Node adjacentNode = GetNode(nextNode.BoardCoordinate + connectionDirection);
                if (adjacentNode != null && IsNodeMillConditionSatisfied(nextNode, adjacentNode))
                {
                    //mill complete
                    mill.Add(adjacentNode);
                    millsList.Add(mill);
                    continue;
                }

                //if no, check opposite direction from original node
                Node oppositeNode = GetNode(node.BoardCoordinate - connectionDirection);
                if (oppositeNode != null && IsNodeMillConditionSatisfied(node, oppositeNode))
                {
                    //found node on opposite side, mill complete
                    mill.Add(oppositeNode);
                    millsList.Add(mill);
                    continue;
                }
            }
        }

        //cull mill list from duplicate mills
        foreach (var mill in millsList)
            for (int i = millsList.Count - 1; i >= 0; i--)
                if (!uniqueMillsList.Contains(mill) && mill.SetEquals(millsList[i]))
                    uniqueMillsList.Add(mill);

        return uniqueMillsList;
    }

    private void SetupBoard()
    {
        _nodes.Clear();
        _boardNodes.Clear();

        for (int ringIndex = 1; ringIndex <= _gameData.RingsAmount; ringIndex++)
        {
            var ring = Instantiate(_ringPrefab, transform);
            ring.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            ring.name = $"Ring_{ringIndex}";

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //center node
                    if (i == 0 && j == 0)
                        continue;

                    Vector2Int boardCoordinate = new(i * ringIndex, j * ringIndex);
                    Vector3 worldPosition = ring.transform.position + new Vector3(boardCoordinate.x * _gameData.PositionOffset, boardCoordinate.y * _gameData.PositionOffset, 0);
                    List<Vector2Int> connectionDirections = GetNodeConnectionDirections(boardCoordinate, ringIndex);

                    BoardNode boardNode = Instantiate(_nodePrefab, ring.transform);
                    boardNode.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
                    boardNode.gameObject.name = $"Node_{boardCoordinate.x}_{boardCoordinate.y}";

                    Node node = new(boardCoordinate, boardNode, ringIndex, connectionDirections);
                    boardNode.SetupBoardNode(node);

                    _nodes.Add(boardCoordinate, node);
                    _boardNodes.Add(boardNode);

                    //testing
                    //boardNode.GetComponent<SpriteRenderer>().color = ringIndex == 1 ? Color.red : (ringIndex == 2 ? Color.green : Color.blue);
                }
            }
        }

        DrawBoardLines();
    }

    private void DrawBoardLines()
    {
        _lines.Clear();

        var lineHolder = Instantiate(_ringPrefab, transform);
        lineHolder.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        lineHolder.gameObject.name = "LineHolder";

        List<HashSet<BoardNode>> allConnections = new();

        foreach (Node node in _nodes.Values)
        {
            for (int i = 0; i < node.ConnectionDirections.Count; i++)
            {
                HashSet<BoardNode> connectedBoardNodes = new()
                {
                    node.BoardNode,
                    _nodes[node.BoardCoordinate + node.ConnectionDirections[i]].BoardNode
                };

                bool contains = false;
                for (int j = allConnections.Count - 1; j >= 0; j--)
                {
                    if (allConnections[j].SetEquals(connectedBoardNodes))
                    {
                        contains = true;
                        break;
                    }
                }

                allConnections.Add(connectedBoardNodes);

                if (contains)
                    continue;

                var line = Instantiate(_linePrefab, lineHolder.transform);
                line.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                line.SetupBoardLine(node.BoardNode, _nodes[node.BoardCoordinate + node.ConnectionDirections[i]].BoardNode);

                _lines.Add(line);
            }
        }
    }

    private List<Vector2Int> GetNodeConnectionDirections(Vector2Int coordinate, int ringIndex)
    {
        List<Vector2Int> results = new();

        int minRingIndex = 1;
        int maxRingIndex = _gameData.RingsAmount;

        //center of the board node
        if (coordinate.Equals(Vector2Int.zero))
        {

        }

        //middle of the line node
        else if (coordinate.x == 0 || coordinate.y == 0)
        {
            //horizontal directions on the same ring
            if (coordinate.x == 0)
            {
                //right
                results.Add(new(ringIndex, 0));
                //left
                results.Add(new(-ringIndex, 0));
            }

            //vertical directions on the same ring
            if (coordinate.y == 0)
            {
                //up
                results.Add(new(0, ringIndex));
                //down
                results.Add(new(0, -ringIndex));
            }

            //check on different rings
            int deltaX = coordinate.x == 0 ? 0 : MathF.Sign(coordinate.x);
            int deltaY = coordinate.y == 0 ? 0 : MathF.Sign(coordinate.y);

            //for higher rings
            if (maxRingIndex > ringIndex)
                results.Add(new(deltaX, deltaY));

            //for lower rings
            if (minRingIndex < ringIndex)
                results.Add(new(-deltaX, -deltaY));
        }

        //corner node
        else
        {
            results.Add(new(-coordinate.x, 0)); //horizontal check: -x, 0 (always other x direction (left/right) and middle of the board on y axis)
            results.Add(new(0, -coordinate.y)); //vertical check: 0, -y (always other y direction (up/down) and middle of the board on x axis)

            //TODO add diagonals flag to data
            if (_gameData.EnableDiagonalLines)
            {
                int deltaX = coordinate.x == 0 ? 0 : MathF.Sign(coordinate.x);
                int deltaY = coordinate.y == 0 ? 0 : MathF.Sign(coordinate.y);

                //for higher rings
                if (maxRingIndex > ringIndex)
                    results.Add(new(deltaX, deltaY));

                //for lower rings
                else if (minRingIndex < ringIndex)
                    results.Add(new(-deltaX, -deltaY));
            }
        }

        return results;
    }

    private bool IsNodeMillConditionSatisfied(Node originNode, Node nextNode)
    {
        return originNode.IsOccupied() && nextNode.IsOccupied() && nextNode.Piece.Owner == originNode.Piece.Owner;
    }
}
