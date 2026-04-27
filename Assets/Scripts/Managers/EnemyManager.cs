using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance {get; private set;}

    [SerializeField] private EnemyController[] _wanderingEnemyPrefabs;
    [SerializeField] private int _maxEnemiesInScene = 5;
    [SerializeField] private int _spawnSquareBound = 100;
    private int _spawnPointXMinBound => -_spawnSquareBound / 2;
    private int _spawnPointXMaxBound => _spawnSquareBound / 2;
    private int _spawnPointZMinBound => -_spawnSquareBound / 2;
    private int _spawnPointZMaxBound => _spawnSquareBound / 2;

    public List<GameObject> EnemiesInScene { get; private set; }= new List<GameObject>();

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

            Vector3 spawnPoint = new Vector3(x: Random.Range(_spawnPointXMinBound, _spawnPointXMaxBound), 
                                             z: Random.Range(_spawnPointZMinBound, _spawnPointZMaxBound),
                                             y: 0);

            EnemiesInScene.Add(Instantiate(enemyToInstantiate, spawnPoint, Quaternion.identity));
        }
    }
}
