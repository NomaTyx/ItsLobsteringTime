using System;
using UnityEngine;

public class Health : MonoBehaviour, IAttackable
{
    //static event to 
    public static event Action<DamageInfo> Damaged;
    public float CurrentHealth => _currentHealth;

    [SerializeField] private float _maxHealth = 1;
    private float _currentHealth;

    public event Action Died;
    
    public bool Damage(DamageInfo info)
    {
        _currentHealth -= info.Amount;
        Damaged?.Invoke(info);

        if (_currentHealth <= 0)
        {
            Died?.Invoke();
        }

        return true;
    }
}
