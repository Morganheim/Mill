using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEventController : MonoBehaviour
{
    [SerializeField] private GameEventEmitter _emitter;

    private bool _isPaused = false;
    private bool _isGameOver = false;

    private void OnEnable()
    {
        _isGameOver = false;
        _isPaused = false;
    }

    public void OnPauseInputReceived(InputAction.CallbackContext context)
    {
        if (!context.performed || _isGameOver)
            return;

        _isPaused = !_isPaused;

        _emitter.Emit(_isPaused ? "OnPauseRequested" : "OnUnpauseRequested");
    }

    public void OnDeselectPieceInputReceived(InputAction.CallbackContext context)
    {
        if (!context.performed || _isGameOver)
            return;

        _emitter.Emit("OnDeselectPiece");
    }

    public void OnGameComplete()
    {
        _isGameOver = true;
    }

    public void OnGamePauseToggle(bool isPaused)
    {
        _isPaused = isPaused;
    }
}
