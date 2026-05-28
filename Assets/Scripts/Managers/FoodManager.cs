using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private Food[] _foodPrefabs;
    [SerializeField] private int _maxFoodInScene;
    [SerializeField] private int _foodRespawnThreshold = 2;
    [SerializeField] private int _spawnSquareBound = 100;
    private int _minX => -_spawnSquareBound / 2;
    private int _maxX => _spawnSquareBound / 2;
    private int _minZ => -_spawnSquareBound / 2;
    private int _maxZ => _spawnSquareBound / 2;

    public static FoodManager Instance { get; private set; }

    public List<GameObject> FoodInScene { get; private set; } = new List<GameObject>();


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There's already a FoodManager singleton in the scene");
        }

        Instance = this;
    }

    public void SpawnFoodUpToMax()
    {
        while (FoodInScene.Count < _maxFoodInScene)
        {
            float spawnPointX = Random.Range(_minX, _maxX);
            float spawnPointZ = Random.Range(_minZ, _maxZ);

            GameObject foodToInstantiate = _foodPrefabs[Random.Range(0, _foodPrefabs.Length)].gameObject;
            Vector3 foodInstantiationLocation = new Vector3(spawnPointX, 0f, spawnPointZ);

            // SamplePosition returns true if a valid point is found on the NavMesh
            if (NavMesh.SamplePosition(foodInstantiationLocation, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                GameObject instantiatedFood = Instantiate(foodToInstantiate, hit.position, Quaternion.identity);
                FoodInScene.Add(instantiatedFood);
            }
        }

    }

    //temporary, i should probably do an event pattern but hey man what are you gonna do
    public bool RemoveFoodFromList(GameObject foodToRemove)
    {
        if(_foodRespawnThreshold >= FoodInScene.Count)
        {
            SpawnFoodUpToMax();
        }
        return FoodInScene.Remove(foodToRemove);
    }
}
