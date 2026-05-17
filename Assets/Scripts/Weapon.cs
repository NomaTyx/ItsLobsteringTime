using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float AttackRange => _attackRange;
    public float AttackCooldownSeconds => _attackCooldownSeconds;
    public float Damage => _attackDamage;
    public AnimatorOverrideController AnimController;

    [SerializeField] protected float _attackRange = 2.5f;
    [SerializeField] protected float _attackCooldownSeconds = 2.5f;
    [SerializeField] private float _attackDamage = 1f;
    [SerializeField] protected Animator _animator;

    protected float _nextAttackTime;

    /// <summary>
    /// Tries to attack which entails doing pre-attack checks such as cooldown. 
    /// If everything is kosher, return true.
    /// </summary>
    public virtual bool TryAttack()
    {
        return !(Time.time < _nextAttackTime);
    }

    public virtual bool TryAttack(IAttackable target)
    {
        return !(Time.time < _nextAttackTime);
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
