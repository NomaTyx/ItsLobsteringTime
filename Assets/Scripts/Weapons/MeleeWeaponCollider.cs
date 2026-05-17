using System;
using UnityEngine;

public class MeleeWeaponCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health hitHealth)) 
        {
            float damageAmount = transform.parent.gameObject.GetComponent<Weapon>().Damage;
            hitHealth.Damage(new DamageInfo(damageAmount, other.gameObject, transform.parent.gameObject));
            gameObject.SetActive(false);
            Debug.Log("damaged " + other.gameObject.name);
        }
    }
}
