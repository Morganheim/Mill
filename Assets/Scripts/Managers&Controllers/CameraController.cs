using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData _gameData;

    [Header("Internal Components")]
    [SerializeField] private Camera _camera;

    public void OnGameStartRequested()
    {
        _camera.orthographicSize = Mathf.RoundToInt(_gameData.RingsAmount * (10 / 3));
    }
}
