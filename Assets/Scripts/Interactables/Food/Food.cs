using System;
using UnityEngine;

[Serializable]
public class Food : AttackTarget
{
    [SerializeField] private float _energyValue = 10f;
    [SerializeField] private int SizeRequirement = (int) LobsterSize.Small;

    public virtual void OnEaten()
    {
        PlayerController.Instance.GetComponent<PlayerEnergy>().GainEnergy(_energyValue);
        Destroy(gameObject);
    }
}
