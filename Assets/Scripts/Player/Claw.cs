using UnityEngine;

/// <summary>
/// The player's default weapon.
/// </summary>
public class Claw : Weapon
{
    [SerializeField] float _attackCooldownSeconds = 0.5f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _attackAngleDeg = 90f;

    /// <summary>
    /// Tries to attack which entails looking through its range to find a target
    /// </summary>
    public override void TryAttack()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, _attackRange, LayerMask.GetMask("Enemy"))) 
        {
            Vector3 positionDiff = c.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, positionDiff);
            
            if (angleToTarget > _attackAngleDeg / 2 || angleToTarget < -_attackAngleDeg / 2)
            {
                continue;
            }

            Debug.Log("Hit " + c.name + "!!");
        }
    }

    /// <summary>
    /// Performs the attack, which may either deal damage directly or spawn a projectile or start an animation.
    /// </summary>
    /// <param name="target"></param>
    protected override void Attack(IAttackable target)
    {

    }
}
