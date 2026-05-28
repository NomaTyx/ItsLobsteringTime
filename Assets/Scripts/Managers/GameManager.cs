using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action<bool> GamePaused;
    public bool Paused => _paused;
    private bool _paused = false;

    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists a GameManager singleton!");
            return;
        }
        Instance = this;
    }

    protected virtual void Start()
    {
        PlayerEnergy.PlayerDead += GameOver;
        FoodManager.Instance.SpawnFoodUpToMax();
        EnemyManager.Instance.SpawnWanderingEnemies();
    }

    private void OnDestroy()
    {
        PlayerEnergy.PlayerDead -= GameOver;
    }

    public void PauseGame()
    {
        if (_paused)
        {
            Time.timeScale = 1.0f;
        }
        else 
        { 
            Time.timeScale = 0.0f;
        }
        _paused = !_paused;
        UI.Instance.SetPauseMenu(_paused);
    }

    public void SetTimeScale(float value)
    {
        if (_paused) return;
        Time.timeScale = value;
    }

    private void GameOver(DeathCause cause)
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(2);
        SceneHandler.Instance.LoadScene("Title Screen");
    }

    private void OnToggleDebug()
    {

    }
}
