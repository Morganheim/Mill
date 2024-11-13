using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNode : MonoBehaviour
{
    [SerializeField] private GameEventEmitter _emitter;
    [SerializeField] private SpriteRenderer _highlight;
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    private Node _node;
    private readonly List<BoardLine> _boardLines = new();

    public Node Node => _node;
    public List<BoardLine> BoardLines => _boardLines;

    public void SetupBoardNode(Node node)
    {
        _node = node;
    }

    public void OnClick()
    {
        _emitter.Emit(new NodeMessage("OnNodeClicked", _node));
    }

    public void OnHoverEnter()
    {
        _emitter.Emit(new NodeMessage("OnNodeHoverEnter", _node));
    }

    public void OnHoverExit()
    {
        _emitter.Emit(new NodeMessage("OnNodeHoverExit", _node));
    }

    public void UpdateColor(Color color)
    {
        SpriteRenderer.color = color;
        _highlight.color = color;
        foreach (var boardLine in _boardLines)
            boardLine.UpdateLineColor();
    }

    public void ToggleHighlight(bool isEnabled, Color color = default)
    {
        if (isEnabled)
            _highlight.color = color;

        _highlight.gameObject.SetActive(isEnabled);
    }
}

public class Node
{
    public Vector2Int BoardCoordinate { get; private set; }
    public List<Vector2Int> ConnectionDirections { get; private set; }
    public int RingIndex { get; private set; }
    public BoardNode BoardNode { get; private set; }
    public PlayerPiece Piece { get; private set; }
    public HashSet<Node> NeighboringNodes { get; private set; }

    public Node(Vector2Int boardCoordinate, BoardNode boardNode, int ringIndex, List<Vector2Int> connectionDirections)
    {
        BoardCoordinate = boardCoordinate;
        BoardNode = boardNode;
        RingIndex = ringIndex;
        ConnectionDirections = connectionDirections;
    }

    public void PlacePiece(PlayerPiece piece)
    {
        Piece = piece;
        piece.PlacePiece(this);

        BoardNode.UpdateColor(piece.Owner.PieceColor);
    }

    public void RemovePiece(bool isDestroyed = false)
    {
        if (Piece == null)
        {
            Debug.LogWarning($"Node {BoardNode.gameObject.name} holds no piece.");
            return;
        }

        Piece.RemovePiece(isDestroyed);
        Piece = null;

        BoardNode.UpdateColor(Color.white);
    }

    public bool IsOccupied()
    {
        return Piece != null;
    }

    public bool IsNeighbor(Node node)
    {
        foreach (var coordinateDirection in ConnectionDirections)
            if (BoardCoordinate + coordinateDirection == node.BoardCoordinate)
                return true;

        return false;
    }
}