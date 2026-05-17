using System;
using UnityEngine;

public class MeleeWeaponCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Weapon _weapon = transform.parent.gameObject.GetComponent<Weapon>();
        if (other.TryGetComponent(out IAttackable hitHealth) && !_weapon.DamagedByThisAttack.Contains(hitHealth))
        {
            float damageAmount = _weapon.Damage;
            hitHealth.Damage(new DamageInfo(damageAmount, other.gameObject, transform.parent.gameObject));
            _weapon.DamagedByThisAttack.Add(hitHealth);
            Debug.Log("damaged " + other.GetType());
        }
    }
}
