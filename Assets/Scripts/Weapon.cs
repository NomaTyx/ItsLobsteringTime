using UnityEngine;

public class Weapon : MonoBehaviour
{
    public virtual void TryAttack()
    {
        Debug.Log("Tried attack with undefined weapon.");
        Attack(null);
    }

    public virtual void Attack(IAttackable target)
    {
        Debug.Log("Attacked with undefined weapon.");
    }
}
