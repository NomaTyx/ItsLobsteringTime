using UnityEngine;

public class Claw : Weapon
{
    /// <summary>
    /// Tries to attack which entails looking through its range to find a target
    /// </summary>
    public override void TryAttack()
    {
        
    }

    /// <summary>
    /// Performs the attack, which may either deal damage directly or spawn a projectile or start an animation.
    /// </summary>
    /// <param name="target"></param>
    public override void Attack(IAttackable target)
    {

    }
}
