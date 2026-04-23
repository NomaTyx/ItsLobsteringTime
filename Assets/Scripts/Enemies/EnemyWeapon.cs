using UnityEngine;

public class EnemyWeapon : Weapon
{
    public float Range {get; private set;} = 3;
    public override void TryAttackWithTarget(IAttackable target)
    {
        Attack(target);
    }

    protected override void Attack(IAttackable target)
    {
        Debug.Log("attacked player with" + gameObject.name);
    }
}
