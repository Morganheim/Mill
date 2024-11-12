using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Mill/Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [field: SerializeField] public string PlayerName { get; private set; }
    [field: SerializeField] public Color PieceColor { get; private set; }

    public Queue<PlayerPiece> AvailablePieces { get; private set; } = new();
    public List<PlayerPiece> PlacedPieces { get; private set; } = new();
    public List<PlayerPiece> DestroyedPieces { get; private set; } = new();
    public List<HashSet<Node>> Mills { get; private set; } = new();

    public void InitPlayer(int initialPiecesAmount)
    {
        AvailablePieces.Clear();
        PlacedPieces.Clear();
        DestroyedPieces.Clear();
        Mills.Clear();

        for (int i = 0; i < initialPiecesAmount; i++)
            AvailablePieces.Enqueue(new PlayerPiece(this));
    }

    public void RemovePieceFromBoard(PlayerPiece piece, Node node, bool isDestroyed = false)
    {
        if (piece == null || !PlacedPieces.Contains(piece))
            return;

        //removes cached mills that contained node
        RemoveMillsWithNode(node);

        if (isDestroyed)
        {
            PlacedPieces.Remove(piece);
            DestroyedPieces.Add(piece);
        }
    }

    public PlayerPiece GetPiece()
    {
        PlayerPiece piece = AvailablePieces.Dequeue();
        PlacedPieces.Add(piece);

        return piece;
    }

    public bool IsMillDuplicate(HashSet<Node> newMill)
    {
        foreach (var mill in Mills)
            if (mill.SetEquals(newMill))
                return true;

        Mills.Add(newMill);

        return false;
    }

    public bool IsNodePartOfAnActiveMill(Node node)
    {
        foreach (var mill in Mills)
            if (mill.Contains(node)) return true;

        return false;
    }

    public bool IsThereAPlacedNonMillPiece()
    {
        for (int i = 0; i < PlacedPieces.Count; i++)
            if (!IsNodePartOfAnActiveMill(PlacedPieces[i].Node))
                return true;

        return false;
    }

    public bool IsLoser()
    {
        return AvailablePieces.Count + PlacedPieces.Count < 3 && DestroyedPieces.Count > 0;
    }

    public override string ToString()
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(PieceColor)}>{PlayerName}</color>";
    }

    private void RemoveMillsWithNode(Node node)
    {
        for (int i = Mills.Count - 1; i >= 0; i--)
            if (Mills[i].Contains(node))
                Mills.RemoveAt(i);
    }
}
