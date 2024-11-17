using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Mill/Data/Audio Data")]
public class AudioData : ScriptableObject
{
    [field:Header("Music Clips")]
    [field: SerializeField] public AudioClip MainMenuMusic { get; private set; }
    [field: SerializeField] public AudioClip GameMusic { get; private set; }

    [field:Header("UI SFX Clips")]
    [field: SerializeField] public AudioClip ButtonClickSFX { get; private set; }

    [field:Header("SFX Clips")]
    [field: SerializeField] public AudioClip PiecePlacementSFX { get; private set; }
    [field: SerializeField] public AudioClip PieceMovementSFX { get; private set; }
    [field: SerializeField] public AudioClip PieceRemoveSFX { get; private set; }
    [field: SerializeField] public AudioClip PieceSelectSFX { get; private set; }
    [field: SerializeField] public AudioClip MillSFX { get; private set; }
    [field: SerializeField] public AudioClip WinSFX { get; private set; }
    [field: SerializeField] public AudioClip DrawSFX { get; private set; }
}
