using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance {get; private set;}

    [SerializeField] private EnemyController[] _wanderingEnemyPrefabs;
    [SerializeField] private int _maxEnemiesInScene = 5;
    [SerializeField] private int _spawnPointXMinBound = -50;
    [SerializeField] private int _spawnPointXMaxBound = 50;
    [SerializeField] private int _spawnPointZMinBound = -50;
    [SerializeField] private int _spawnPointZMaxBound = 50;

    public List<GameObject> EnemiesInScene {get; private set; }= new List<GameObject>();

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists an EnemyManager singleton!");
            return;
        }

        Instance = this;
    }

    public void SpawnWanderingEnemies()
    {
        for(int i = 0; i < _maxEnemiesInScene; i++)
        {
            GameObject enemyToInstantiate = _wanderingEnemyPrefabs[Random.Range(0, _wanderingEnemyPrefabs.Length)].gameObject;

            Vector3 spawnPoint = new Vector3(Random.Range(_spawnPointXMinBound, _spawnPointXMaxBound), 
                                            0, 
                                            Random.Range(_spawnPointZMinBound, _spawnPointZMinBound));

            EnemiesInScene.Add(Instantiate(enemyToInstantiate, spawnPoint, Quaternion.identity));
        }
    }
}
