using UnityEngine;

public class EnemyClaw : Weapon
{
    public override bool TryAttack(IAttackable target)
    {
        if(!base.TryAttack(target)) return false;
        Attack(target);
        return true;
    }

    protected override void Attack(params IAttackable[] targets)
    {
        _animator.Play("attack");
        base.Attack(targets);
    }
}
