using System;
using UnityEngine;

public class ClawCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health hitHealth)) 
        {
            float damageAmount = transform.parent.gameObject.GetComponent<Claw>().Damage;
            hitHealth.Damage(new DamageInfo(damageAmount, other.gameObject, transform.parent.gameObject));
        }
        gameObject.SetActive(false);
    }
}
