using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The player's default weapon.
/// </summary>
public class Claw : Weapon
{   
    [SerializeField] private float _attackEnergyCost = 10f;

    /// <summary>
    /// Tries to attack which entails looking through its range to find a target
    /// </summary>
    public override bool TryAttack()
    {
        if(!base.TryAttack()) return false;
        Attack();
        return true;
    }

    protected override void Attack()
    {
        _animator.Play("attack");
        PlayerEnergy.Instance.ConsumeEnergy(_attackEnergyCost);
        base.Attack();
    }
}
