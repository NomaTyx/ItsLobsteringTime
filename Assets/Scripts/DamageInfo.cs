using UnityEngine;

public class DamageInfo
{
    public float Amount { get; private set; }
    public Controller Target { get; private set; }
    public Controller Instigator { get; private set; }

    public DamageInfo(float amount, Controller target, Controller instigator)
    {
        if(instigator.Size > target.Size)
        {
            Amount = amount * (instigator.Size - target.Size + 1);
        }
        else
        {
            Amount = amount / (target.Size - instigator.Size + 1);
        }
        Debug.Log($"Base damage: {amount}, Instigator size: {instigator.Size}, Target size: {target.Size}, Final damage: {Amount}");
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
