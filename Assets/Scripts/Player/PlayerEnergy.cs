using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public static PlayerEnergy Instance;
    public int PlayerSize => _currentSize;

    public event Action PlayerDamaged;
    public event Action PlayerDead;

    [SerializeField] private int _maxEnergy = 100;
    [SerializeField] private float _energyDrainPerSecond = 5;
    [SerializeField] private float[] _playerGrowthEnergyCosts;
    [SerializeField] private float _playerGrowthPercent = 0.15f;
    private int _currentSize = 0;

    private float _currentEnergy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There already exists a PlayerEnergy instance");
            return;
        }

        Instance = this;
    }

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
        PlayerDamaged.Invoke();
    }

    private void Die()
    {
        Debug.Log("u ded lole");
    }

    void FixedUpdate()
    {
        //for the love of GOD please remember to change this later
        //Debug.Log($"Current energy: {_currentEnergy}");
        _currentEnergy -= _energyDrainPerSecond * Time.fixedDeltaTime;
    }
}

/// <summary>
/// This enum exists just to add semantic meaning to certain words so i can avoid using magic numbers.
/// </summary>
public enum LobsterSize
{
    Small,
    Medium,
    Large
}