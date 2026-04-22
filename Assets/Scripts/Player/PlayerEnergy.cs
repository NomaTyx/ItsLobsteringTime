using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour, IAttackable
{
    public static PlayerEnergy Instance;
    public int PlayerSize => _currentSize;
    public float Energy => _currentEnergy;
    public float MaxEnergy => _maxEnergy;

    [Header("Energy")]
    [SerializeField] private int _maxEnergy = 100;
    [SerializeField] private float _energyDrainPerSecond = 1;

    [Header("Growth")]
    [SerializeField] private float _maxMoltTimerSeconds = 60f;
    [SerializeField] private float[] _playerGrowthEnergyCosts;
    [SerializeField] private float _playerGrowthPercent = 0.15f;

    [SerializeField] private float _eatRange = 5;

    public event Action PlayerDamaged;
    public event Action PlayerDead;

    private int _currentSize = 0;
    private float _currentEnergy;

    private float _moltTimerSeconds;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There already exists a PlayerEnergy instance");
            return;
        }

        Instance = this;

        _moltTimerSeconds = _maxMoltTimerSeconds;
    }

    private void Start()
    {
        _currentEnergy = _maxEnergy;
    }

    public void GainEnergy(float amount)
    {
        _currentEnergy = Mathf.Min(_currentEnergy + amount, _maxEnergy);
    }

    public void TryGrow()
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
        _currentEnergy -= _playerGrowthEnergyCosts[Math.Min(_currentSize, _playerGrowthEnergyCosts.Length - 1)];
        Debug.Log("Grew!");
    }

    public virtual void Eat()
    {
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

        if (closestFood == null) return;

        GainEnergy(closestFood.EnergyValue);
        closestFood.OnEaten();
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
        _currentEnergy -= _energyDrainPerSecond * Time.fixedDeltaTime;

        _moltTimerSeconds -= Time.deltaTime;

        if (_moltTimerSeconds <= 0)
        {
            TryGrow();
            _moltTimerSeconds = _maxMoltTimerSeconds;
        }
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