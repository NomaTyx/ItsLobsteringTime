using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float _energyValue = 10f;

    public void onEaten()
    {
        Debug.Log("Food was eaten");
        Destroy(gameObject);
    }
}
