using System.Linq;
using UnityEngine;

/// <summary>
/// The player's default weapon.
/// </summary>
public class Claw : Weapon
{
    [SerializeField] float _attackCooldownSeconds = 0.5f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _attackAngleDeg = 90f;
    [SerializeField] float _attackDamage = 1f;

    /// <summary>
    /// Tries to attack which entails looking through its range to find a target
    /// </summary>
    public override bool TryAttack()
    {
        if (!base.TryAttack()) return false;

        IAttackable[] targets = Physics.OverlapSphere(transform.position, _attackRange, LayerMask.GetMask("Enemy")).Where((c) =>
        {
            Vector3 positionDiff = c.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, positionDiff);
            return !(angleToTarget > _attackAngleDeg / 2 || angleToTarget < -_attackAngleDeg / 2);
        }).
        Where((c) => c.TryGetComponent(out Health hitHealth)).Select(c => c.GetComponent<IAttackable>()).ToArray();


        // foreach (Collider c in Physics.OverlapSphere(transform.position, _attackRange, LayerMask.GetMask("Enemy")))
        // {
        //     Vector3 positionDiff = c.transform.position - transform.position;
        //     float angleToTarget = Vector3.Angle(transform.forward, positionDiff);

        //     if (angleToTarget > _attackAngleDeg / 2 || angleToTarget < -_attackAngleDeg / 2)
        //     {
        //         continue;
        //     }

        //     if (c.gameObject.TryGetComponent<Health>(out Health hitHealth))
        //     {
        //         hitHealth.Damage(new DamageInfo(_attackDamage, hitHealth.gameObject, gameObject));
        //     }
        //     Debug.Log("Hit " + c.name + "!!");
        // }

        Attack(targets);

        return true;
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Attack(params IAttackable[] targets)
    {
        foreach(IAttackable enemy in targets)
        {
            enemy.Damage(new DamageInfo(_attackDamage, gameObject, gameObject));
        }
        
        base.Attack(targets);
    }
}
