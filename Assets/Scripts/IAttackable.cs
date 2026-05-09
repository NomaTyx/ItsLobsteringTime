using UnityEngine;

/// <summary>
/// An interface for anything that can be attacked, enemy or player.
/// </summary>
public interface IAttackable
{
    void Damage(DamageInfo info);
}