using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float AttackRange {get; protected set;} = 2.5f;
    public float AttackCooldownSeconds {get; protected set;} = 2.5f;
    public AnimatorOverrideController AnimController;

    protected float _nextAttackTime;

    /// <summary>
    /// Tries to attack which entails doing pre-attack checks such as cooldown. 
    /// If everything is kosher, return true.
    /// </summary>
    public virtual bool TryAttack()
    {
        if (Time.time < _nextAttackTime) return false;
        
        return true;
    }

    public virtual bool TryAttack(IAttackable target)
    {
        if (Time.time < _nextAttackTime) return false;
        return true;
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
