using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece
{
    public PlayerPiece(PlayerData owner)
    {
        Owner = owner;
    }

    public PlayerData Owner { get; private set; }
    public Node Node { get; private set; }

    public void PlacePiece(Node node)
    {
        Node = node;
    }

    public void RemovePiece(bool isDestroyed = false)
    {
        Owner.RemovePieceFromBoard(this, Node, isDestroyed);
        Node = null;
    }
}
