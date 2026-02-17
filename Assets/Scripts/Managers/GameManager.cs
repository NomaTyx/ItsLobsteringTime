using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("There already exists a GameManager singleton!");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        FoodManager.instance.SpawnFoodUpToMax();
    }
}
