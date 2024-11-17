using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Mill/Data/Game Data")]
public class GameData : ScriptableObject
{
    [field: Header("Fixed Values")]
    [field: SerializeField] public int PositionOffset { get; private set; }
    [field: SerializeField, Range(0.1f, 100)] public float NotificationTypeSpeed { get; private set; }
    [field: SerializeField] public float NotificationLifetime { get; private set; }
    [field: SerializeField, Range(0, 1)] public float OwnedNodeHighlightTransparency { get; private set; }
    [field: SerializeField] public int AudioPoolSize { get; private set; }

    [field: Header("Changable Values")]
    [field: SerializeField, Range(1, 10)] public int RingsAmount { get; private set; }
    [field: SerializeField, Range(3, 20)] public int PiecesInitialAmount { get; private set; }
    [field: SerializeField] public bool IsDiagonalBoardLinesEnabled { get; private set; }
    [field: SerializeField] public bool IsPiecesFlyEnabled { get; private set; }
    [field: SerializeField] public bool IsMiddlePositionEnabled { get; private set; }
    [field: SerializeField, Range(0, 1)] public float MusicVolume { get; private set; }
    [field: SerializeField, Range(0, 1)] public float SFXVolume { get; private set; }

    public void UpdateSFXVolume(float amount)
    {

        SFXVolume = amount;
    }

    public void UpdateMusicVolume(float amount)
    {
        MusicVolume = amount;
    }

    public void UpdateRingsAmount(int amount)
    {
        RingsAmount = amount;
    }

    public void UpdateInitialPiecesAmount(int amount)
    {
        PiecesInitialAmount = amount;
    }

    public void EnableBoardDiagonalLines(bool enable)
    {
        IsDiagonalBoardLinesEnabled = enable;
    }

    public void EnableFlyingPieces(bool enable)
    {
        IsPiecesFlyEnabled = enable;
    }

    public void EnableCenterNode(bool enable)
    {
        IsMiddlePositionEnabled = enable;
    }
}
