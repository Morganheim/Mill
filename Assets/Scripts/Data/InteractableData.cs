using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "InteractableData", menuName = "Mill/Data/Interactable Data")]
public class InteractableData : ScriptableObject
{
    [field: SerializeField] public PointerEventData.InputButton[] AllowedMouseKeys { get; private set; }
    [field: SerializeField, Tooltip("Ray distance from camera near clip plane")] public float Distance { get; private set; }
}