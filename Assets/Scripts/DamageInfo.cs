using UnityEngine;

public class DamageInfo
{
    public float Amount { get; private set; }
    public GameObject Target { get; private set; }
    public GameObject Instigator { get; private set; }

    public DamageInfo(float amount, GameObject target, GameObject instigator)
    {
        Amount = amount;
        Target = target;
        Instigator = instigator;
    }

    public void SetAmount(float amount)
    {
        Amount = amount;
    }
}
