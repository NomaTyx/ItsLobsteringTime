using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Canvas UICanvas;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists a GameManager singleton!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UICanvas = FindFirstObjectByType<Canvas>();
        PlayerEnergy.PlayerDead += GameOver;

        FoodManager.Instance.SpawnFoodUpToMax();
        EnemyManager.Instance.SpawnWanderingEnemies();
    }

    private void OnDestroy()
    {
        PlayerEnergy.PlayerDead -= GameOver;
    }

    private void GameOver()
    {

    }

    private void OnToggleDebug()
    {

    }
}
