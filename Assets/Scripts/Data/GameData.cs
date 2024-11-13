using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Mill/Data/Game Data")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public int PositionOffset { get; private set; }
    [field: SerializeField, Range(1, 10)] public int RingsAmount { get; private set; }
    [field: SerializeField, Range(3, 20)] public int PiecesInitialAmount { get; private set; }
    [field: SerializeField] public bool EnableDiagonalLines { get; private set; }
    [field: SerializeField] public bool EnablePiecesAlwaysFly { get; private set; }
    [field: SerializeField] public bool EnableMiddlePosition { get; private set; }
    [field: SerializeField, Range(0.1f, 100)] public float NotificationTypeSpeed { get; private set; }
    [field: SerializeField] public float NotificationLifetime { get; private set; }
    [field: SerializeField, Range(0, 1)] public float OwnedNodeHighlightTransparency { get; private set; }
}
