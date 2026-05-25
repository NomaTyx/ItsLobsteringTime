using UnityEngine;

public class DamageInfo
{
    public float Amount { get; private set; }
    public Controller Target { get; private set; }
    public Controller Instigator { get; private set; }

    public DamageInfo(float amount, Controller target, Controller instigator)
    {
        Amount = amount * instigator.Size / target.Size;
        Target = target;
        Instigator = instigator;
    }

    public void SetAmount(float amount)
    {
        Amount = amount;
    }

    public override string ToString()
    {
        return $"{Instigator.name} did {Amount} damage to {Target.name}";
    }
}
