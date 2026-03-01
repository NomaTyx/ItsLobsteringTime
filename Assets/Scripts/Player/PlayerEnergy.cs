using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public event Action PlayerDied;
    public int PlayerSize => _currentSize;

    [SerializeField] private int _maxEnergy = 100;
    [SerializeField] private float _energyDrainPerSecond = 5;
    [SerializeField] private float[] _playerGrowthEnergyCosts;
    [SerializeField] private float _playerGrowthPercent = 0.15f;
    private int _currentSize = 0;

    private float _currentEnergy;

    private void Start()
    {
        _currentEnergy = _maxEnergy;

        GameManager.Instance.MoltTimerExpired += TryGrow;
    }

    public void GainEnergy(float amount)
    {
        _currentEnergy = Mathf.Min(_currentEnergy + amount, _maxEnergy);
    }

    private void TryGrow()
    {
        if(_currentEnergy > _playerGrowthEnergyCosts[Math.Min(_currentSize, _playerGrowthEnergyCosts.Length - 1)])
        {
            Grow();
        }
        else
        {
            Die();
        }
    }

    private void Grow()
    {
        _currentSize++;
        transform.localScale = Vector3.one * (1 + _playerGrowthPercent * _currentSize);
        Debug.Log("Grew!");
    }

    public void TakeDamage(float damageAmount)
    {
        _currentEnergy -= damageAmount;
    }

    private void Die()
    {
        Debug.Log("u ded lole");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //for the love of GOD please remember to change this later
        //Debug.Log($"Current energy: {_currentEnergy}");
        _currentEnergy -= _energyDrainPerSecond * Time.fixedDeltaTime;
    }
}

public static class LobsterSizesExtension
{
    public static LobsterSizes NextSize(LobsterSizes currentSize)
    {
        if (currentSize == LobsterSizes.Large)
        {

        }

        foreach (LobsterSizes size in Enum.GetValues(typeof(LobsterSizes)))
        {
            if (size > currentSize)
            {
                return size;
            }
        }
        return LobsterSizes.Small;
    }
}
public enum LobsterSizes
{
    Small = 0,
    Medium = 1,
    Large = 2,
}