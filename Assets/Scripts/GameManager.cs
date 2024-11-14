using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameEventEmitter _emitter;

    public async void OnGameStartRequested()
    {
        await SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

        _emitter.Emit("OnGameLoaded");
    }

    public void OnGameRestartRequested()
    {
        OnGameUnloadRequested();

        OnGameStartRequested();
    }

    public async void OnGameUnloadRequested()
    {
        await SceneManager.UnloadSceneAsync("GameScene");

        _emitter.Emit("OnGameUnloaded");
    }

    public void OnQuitGameRequested()
    {
        Application.Quit();
    }

    public void OnPauseRequested()
    {
        _emitter.Emit("OnGamePause");
    }

    public void OnUnpauseRequested()
    {
        _emitter.Emit("OnGameUnpause");
    }
}
