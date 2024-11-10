using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNode : MonoBehaviour
{
    [SerializeField] private GameEventEmitter _emitter;

    private Node _node;
    private List<BoardLine> _boardLines;

    public Node Node { get => _node; }

    public void SetupBoardNode(Node node)
    {
        _node = node;
    }

    public void OnClick()
    {
        _emitter.Emit(new NodeMessage("OnNodeClicked", _node));
    }
}


public class Node
{
    public Vector2Int BoardCoordinate { get; private set; }
    public int RingIndex { get; private set; }
    public BoardNode BoardNode { get; private set; }
    public PlayerPiece Piece { get; private set; }
    public HashSet<Node> NeighboringNodes { get; private set; }

    public Node(Vector2Int boardCoordinate, BoardNode boardNode, int ringIndex)
    {
        BoardCoordinate = boardCoordinate;
        BoardNode = boardNode;
        RingIndex = ringIndex;
    }

    public void PlacePiece(PlayerPiece piece)
    {
        Piece = piece;
    }

    public void RemovePiece()
    {
        if (Piece == null)
        {
            Debug.LogWarning($"Node {BoardNode.gameObject.name} holds no piece.");
            return;
        }

        Piece = null;
    }

    public bool IsOccupied()
    {
        return Piece != null;
    }
}