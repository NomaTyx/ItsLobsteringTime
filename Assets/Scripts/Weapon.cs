using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float AttackRange {get; private set;} = 2.5f;
    public float AttackCooldownSeconds {get; private set;} = 2.5f;
    protected float _nextAttackTime;

    /// <summary>
    /// Tries to attack which entails doing pre-attack checks such as cooldown. 
    /// If everything is kosher, return true.
    /// </summary>
    public virtual bool TryAttack()
    {
        Debug.Log("Tried attack with undefined weapon.");
        return true;
    }

    public virtual void TryAttack(IAttackable target)
    {
        Debug.Log("Tried attackWithTarget with undefined weapon");
    }

    /// <summary>
    /// Performs the attack, which may either deal damage directly or spawn a projectile or start an animation.
    /// </summary>
    /// <param name="target"></param>
    protected virtual void Attack()
    {
        _nextAttackTime = Time.time + AttackCooldownSeconds;
    }

    protected virtual void Attack(params IAttackable[] target)
    {
        _nextAttackTime = Time.time + AttackCooldownSeconds;
    }
}
