using System;
using UnityEngine;

public class Health : MonoBehaviour, IAttackable
{
    public float CurrentHealth => _currentHealth;

    [SerializeField] private float _maxHealth = 1;
    private float _currentHealth;

    public event Action Died;
    
    public void Damage(DamageInfo info)
    {
        _currentHealth -= info.Amount;

        if (_currentHealth <= 0)
        {
            Died?.Invoke();
        }
    }
}
