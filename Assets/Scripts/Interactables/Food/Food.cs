using System;
using UnityEngine;

[Serializable]
public class Food : AttackTarget
{
    [SerializeField] private float _energyValue = 10f;

    public virtual void OnEaten()
    {
        PlayerController.Instance.GetComponent<PlayerEnergy>().GainEnergy(_energyValue);
        Destroy(gameObject);
    }
}
