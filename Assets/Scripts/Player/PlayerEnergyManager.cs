using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEnergyManager : MonoBehaviour, IAttackable
{
    public static PlayerEnergyManager Instance;
    public int PlayerSize => _currentSize;

    public event Action PlayerDamaged;
    public event Action PlayerDead;

    [SerializeField] private int _maxEnergy = 100;
    [SerializeField] private float _energyDrainPerSecond = 5;
    [SerializeField] private float[] _playerGrowthEnergyCosts;
    [SerializeField] private float _playerGrowthPercent = 0.15f;
    [SerializeField] private float _eatRange = 5;

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

    public virtual void Eat()
    {
        //TODO: add separate logic for enemies vs food
        Food closestFood = null;
        float smallestDistance = Mathf.Infinity;

        //TODO: Implement cooldown????
        foreach (Collider c in Physics.OverlapSphere(transform.position, _eatRange))
        {
            Food target = c.GetComponent<Food>();

            if (target == null) continue;

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < smallestDistance)
            {
                closestFood = target;
                smallestDistance = distanceToTarget;
            }
        }

        closestFood?.OnEaten();
    }

    public void Damage(float damageAmount)
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