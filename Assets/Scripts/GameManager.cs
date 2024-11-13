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

    public async void OnGameRestartRequested()
    {
        await SceneManager.UnloadSceneAsync("GameScene");

        OnGameStartRequested();
    }

    public void OnMainMenuRequested()
    {
        SceneManager.UnloadSceneAsync("GameScene");
    }

    public void OnQuitGameRequested()
    {
        Application.Quit();
    }
}
