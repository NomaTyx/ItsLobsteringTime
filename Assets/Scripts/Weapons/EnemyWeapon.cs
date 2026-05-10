using UnityEngine;

public class EnemyWeapon : Weapon
{
    public float Range {get; private set;} = 3;
    public override bool TryAttack(IAttackable target)
    {
        if(!base.TryAttack(target)) return false;
        Attack(target);
        return true;
    }

    protected override void Attack(params IAttackable[] targets)
    {
        base.Attack(targets);
        Debug.Log("attacked player with" + gameObject.name);
    }
}
