using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance {get; private set;}

    [Header("Enemy prefabs")]
    [SerializeField] private EnemyController[] _wanderingEnemyPrefabs;
    [SerializeField] private EnemyController[] _territorialEnemyPrefabs;

    [Header("Enemy spawn numbers")]
    [SerializeField] private int _maxWanderingEnemiesInScene = 5;
    [SerializeField] private int _enemyRespawnThreshold = 2;
    [SerializeField] private int _maxTerritorialEnemiesInScene = 5;
    [SerializeField] private int _spawnSquareBound = 100;
    private int _spawnPointXMinBound => -_spawnSquareBound / 2;
    private int _spawnPointXMaxBound => _spawnSquareBound / 2;
    private int _spawnPointZMinBound => -_spawnSquareBound / 2;
    private int _spawnPointZMaxBound => _spawnSquareBound / 2;

    public List<GameObject> EnemiesInScene { get; private set; } = new List<GameObject>();

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
        while (EnemiesInScene.Count < _maxWanderingEnemiesInScene)
        {
            EnemyController enemyToInstantiate = _wanderingEnemyPrefabs[Random.Range(0, _wanderingEnemyPrefabs.Length)];

            Vector3 spawnPoint = new Vector3(x: Random.Range(_spawnPointXMinBound, _spawnPointXMaxBound), 
                                             z: Random.Range(_spawnPointZMinBound, _spawnPointZMaxBound),
                                             y: 0);

            if (NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                int playerSize = PlayerEnergy.Instance.PlayerSize;
                int size = Random.Range(0, 100) switch
                {
                    < 70 => playerSize,
                    < 80 => playerSize - 1,
                    < 90 => playerSize + 1,
                    < 95 => playerSize + 2,
                    _ => playerSize - 2
                };
                enemyToInstantiate.SetSize(size);
                EnemiesInScene.Add(Instantiate(enemyToInstantiate.gameObject, spawnPoint, Quaternion.identity));
            }
        }
    }

    public void RemoveEnemyFromScene(GameObject enemy)
    {
        EnemiesInScene.Remove(enemy);

        if(EnemiesInScene.Count < _enemyRespawnThreshold)
        {
            SpawnWanderingEnemies();
        }
    }
}
