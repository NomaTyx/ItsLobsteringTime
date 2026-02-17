using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject Player;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("There already exists a GameManager singleton!");
            return;
        }

        instance = this;
        Player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    private void Start()
    {
        FoodManager.instance.SpawnFoodUpToMax();
    }
}
