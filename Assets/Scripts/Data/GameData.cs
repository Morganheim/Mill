using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Mill/Data/Game Data")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public int PositionOffset { get; private set; }
    [field: SerializeField] public int RingsAmount { get; private set; }
    [field: SerializeField] public bool EnableDiagonalLines { get; private set; }
    [field: SerializeField] public bool EnablePiecesAlwaysFly { get; private set; }
    [field: SerializeField, Range(1, 100)] public int NotificationTypeSpeed { get; private set; }
}
