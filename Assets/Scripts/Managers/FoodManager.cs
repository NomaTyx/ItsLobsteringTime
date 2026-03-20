using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private Food[] _foodPrefabs;
    [SerializeField] private int _maxFoodInScene;
    [SerializeField] private BoxCollider _floorCollider;

    public static FoodManager Instance { get; private set; }

    private List<GameObject> _foodInScene;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There's already a FoodManager singleton in the scene");
        }

        Instance = this;

        _foodInScene = new List<GameObject>();
    }

    public void SpawnFoodUpToMax()
    {
        while (_foodInScene.Count < _maxFoodInScene)
        {
            float spawnPointX = Random.Range(_floorCollider.bounds.min.x, _floorCollider.bounds.max.x);
            float spawnPointZ = Random.Range(_floorCollider.bounds.min.z, _floorCollider.bounds.max.z);

            GameObject foodToInstantiate = _foodPrefabs[Random.Range(0, _foodPrefabs.Length)].gameObject;
            Vector3 foodInstantiationLocation = new Vector3(spawnPointX, 0.25f, spawnPointZ);

            _foodInScene.Add(Instantiate(foodToInstantiate, foodInstantiationLocation, Quaternion.identity));
        }

    }
}
