using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNode : MonoBehaviour
{
    [SerializeField] private GameEventEmitter _emitter;
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    [SerializeField]private Node _node;
    private List<BoardLine> _boardLines = new();

    public Node Node { get => _node; }
    public List<BoardLine> BoardLines { get => _boardLines; set => _boardLines = value; }

    public void SetupBoardNode(Node node)
    {
        _node = node;
    }

    public void OnClick()
    {
        _emitter.Emit(new NodeMessage("OnNodeClicked", _node));
    }

    public void UpdateColor(Color color)
    {
        SpriteRenderer.color = color;

        foreach (var boardLine in _boardLines)
            boardLine.UpdateLineColor();
    }
}

[System.Serializable]
public class Node
{
    [field:SerializeField]public Vector2Int BoardCoordinate { get; private set; }
    [field:SerializeField]public List<Vector2Int> ConnectionDirections { get; private set; }
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

    public void RemovePiece()
    {
        if (Piece == null)
        {
            Debug.LogWarning($"Node {BoardNode.gameObject.name} holds no piece.");
            return;
        }

        Piece.RemovePiece();
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