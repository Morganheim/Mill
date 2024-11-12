using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData _gameData;

    [Header("Internal Components")]
    [SerializeField] private Camera _camera;

    private void OnEnable()
    {
        _camera.orthographicSize = Mathf.RoundToInt(_gameData.RingsAmount * (10 / 3));
    }
}
