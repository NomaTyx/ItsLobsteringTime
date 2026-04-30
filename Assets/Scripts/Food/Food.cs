using System;
using UnityEngine;

[Serializable]
public class Food : MonoBehaviour
{
    public float EnergyValue => _energyValue;

    [SerializeField] private float _energyValue = 10f;
    [SerializeField] private int _size = (int) LobsterSize.Small;

    public virtual void TryEat(Controller eater)
    {
        if(eater.Size >= _size)
        {
            OnEaten();
        }
    }

    public virtual void OnEaten()
    {
        FoodManager.Instance.RemoveFoodFromList(gameObject);
        Destroy(gameObject);
    }
}
