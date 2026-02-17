using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private Food[] _foodPrefabs;
    [SerializeField] private int _maxFoodInScene;
    [SerializeField] private BoxCollider _floorCollider;

    public static FoodManager instance;

    private List<GameObject> _foodInScene;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There's already a FoodManager singleton in the scene");
        }

        instance = this;

        _foodInScene = new List<GameObject>();
    }

    public void SpawnFoodUpToMax()
    {
        while (_foodInScene.Count < _maxFoodInScene)
        {
            float spawnPointX = Random.Range(_floorCollider.bounds.min.x, _floorCollider.bounds.max.x);
            float spawnPointZ = Random.Range(_floorCollider.bounds.min.z, _floorCollider.bounds.max.z);

            _foodInScene.Add(Instantiate(_foodPrefabs[Random.Range(0, _foodPrefabs.Length)].gameObject,
                new Vector3(spawnPointX, 0.25f, spawnPointZ), Quaternion.identity));
        }

    }
}
