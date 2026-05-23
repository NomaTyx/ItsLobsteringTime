using System;
using UnityEngine;

[Serializable]
public class Food : MonoBehaviour
{
    public float EnergyValue => _energyValue;
    public int Size => _size;

    [SerializeField] private float _energyValue = 10f;
    [SerializeField] private int _size = 1;
    [SerializeField] private FoodRarity _rarity;

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

public enum FoodRarity
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
}
