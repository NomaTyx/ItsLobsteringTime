using UnityEngine;

public class Claw : Weapon
{
    [SerializeField] float _attackCooldownSeconds = 0.5f;
    [SerializeField] float _attackRange = 2f;

    /// <summary>
    /// Tries to attack which entails looking through its range to find a target
    /// </summary>
    public override void TryAttack()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, _attackRange)) 
        {

        }
    }

    /// <summary>
    /// Performs the attack, which may either deal damage directly or spawn a projectile or start an animation.
    /// </summary>
    /// <param name="target"></param>
    public override void Attack(IAttackable target)
    {

    }
}
