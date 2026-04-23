using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float AttackRange {get; private set;} = 2.5f;

    public virtual void TryAttack()
    {
        Debug.Log("Tried attack with undefined weapon.");
    }

    public virtual void TryAttackWithTarget(IAttackable target)
    {
        Debug.Log("Tried attackWithTarget with undefined weapon");
    }

    protected virtual void Attack(IAttackable target)
    {
        Debug.Log("Attacked with undefined weapon.");
    }
}
