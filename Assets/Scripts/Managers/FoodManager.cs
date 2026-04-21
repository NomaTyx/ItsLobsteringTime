using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private Food[] _foodPrefabs;
    [SerializeField] private int _maxFoodInScene;
    [SerializeField] private BoxCollider _floorCollider;

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
            float spawnPointX = Random.Range(_floorCollider.bounds.min.x, _floorCollider.bounds.max.x);
            float spawnPointZ = Random.Range(_floorCollider.bounds.min.z, _floorCollider.bounds.max.z);

            GameObject foodToInstantiate = _foodPrefabs[Random.Range(0, _foodPrefabs.Length)].gameObject;
            Vector3 foodInstantiationLocation = new Vector3(spawnPointX, 0.25f, spawnPointZ);

            FoodInScene.Add(Instantiate(foodToInstantiate, foodInstantiationLocation, Quaternion.identity));
        }

    }

    //temporary, i should probably do an event pattern but hey man what are you gonna do
    public bool RemoveFoodFromList(GameObject foodToRemove)
    {
        return FoodInScene.Remove(foodToRemove);
    }
}
