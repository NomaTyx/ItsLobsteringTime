using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// The player's default weapon.
/// </summary>
public class Claw : Weapon
{
    [SerializeField] private float _attackCooldownSeconds = 0.5f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackAngleDeg = 90f;
    [SerializeField] private float _attackDamage = 1f;
    [SerializeField] private float _attackEnergyCost = 10f;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponentInParent<Animator>();
    }

    /// <summary>
    /// Tries to attack which entails looking through its range to find a target
    /// </summary>
    public override bool TryAttack()
    {
        if (!base.TryAttack()) return false;

        IAttackable[] targets = Physics.OverlapSphere(
                                    transform.position,
                                    _attackRange,
                                    LayerMask.GetMask("Enemy"),
                                    QueryTriggerInteraction.Ignore).
                                Where((c) =>
                                {
                                    Vector3 positionDiff = c.transform.position - transform.position;
                                    float angleToTarget = Vector3.Angle(transform.forward, positionDiff);
                                    return !(angleToTarget > _attackAngleDeg / 2 || angleToTarget < -_attackAngleDeg / 2);
                                }).
                                Where((c) => c.TryGetComponent(out Health hitHealth)).Select(c => c.GetComponent<IAttackable>()).ToArray();

        if (targets.Length == 0) return false;

        Attack(targets);

        return true;
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Attack(params IAttackable[] targets)
    {
        foreach (IAttackable enemy in targets)
        {
            enemy.Damage(new DamageInfo(_attackDamage, gameObject, gameObject));
        }
        _animator.Play("lobster_attack");
        PlayerEnergy.Instance.ConsumeEnergy(_attackEnergyCost);

        base.Attack(targets);
    }
}
