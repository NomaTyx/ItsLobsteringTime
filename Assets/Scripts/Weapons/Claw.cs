using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The player's default weapon.
/// </summary>
public class Claw : Weapon
{
    public float Damage => _attackDamage;

    [SerializeField] private float _attackCooldownSeconds = 0.5f;
    [SerializeField] private float _attackDamage = 1f;
    [SerializeField] private float _attackEnergyCost = 10f;

    [SerializeField] private Animator _animator;

    /// <summary>
    /// Tries to attack which entails looking through its range to find a target
    /// </summary>
    public override bool TryAttack()
    {
        Attack();
        return true;
    }

    protected override void Attack()
    {
        _animator.Play("lobster_attack");
        PlayerEnergy.Instance.ConsumeEnergy(_attackEnergyCost);
        base.Attack();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P)) {
            _animator.Play("lobster_attack");
        }
    }
}
