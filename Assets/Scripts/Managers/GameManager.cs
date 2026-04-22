using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        FoodManager.Instance.SpawnFoodUpToMax();
        EnemyManager.Instance.SpawnWanderingEnemies();
    }
}
