using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Mill/Data/Game Data")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public int PositionOffset { get; private set; }
    [field: SerializeField] public int RingsAmount { get; private set; }
}