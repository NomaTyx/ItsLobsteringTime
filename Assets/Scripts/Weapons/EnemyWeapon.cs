using UnityEngine;

public class EnemyWeapon : Weapon
{
    public float Range {get; private set;} = 3;
    public override void TryAttack(IAttackable target)
    {
        Attack(target);
    }

    protected override void Attack(params IAttackable[] targets)
    {
        base.Attack(targets);
        Debug.Log("attacked player with" + gameObject.name);
    }
}
