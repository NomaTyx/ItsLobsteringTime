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
    [SerializeField] private float _zeroEnergyDeathTimerSeconds = 10f;

    [Header("Growth")]
    [SerializeField] private float _timeBeforeMolt = 20f;
    [SerializeField] private float[] _playerGrowthEnergyCosts;
    [SerializeField] private float _playerGrowthPercent = 0.15f;

    [SerializeField] private float _eatRange = 5;

    public event Action PlayerDamaged;
    public event Action PlayerDead;

    private int _currentSize = 0;
    private float _currentEnergy;
    private bool _dyingFromNoEnergy = false;
    private float _timeToDie;

    private float _moltTimerSeconds;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There already exists a PlayerEnergy instance");
            return;
        }

        Instance = this;

        _moltTimerSeconds = _timeBeforeMolt;
    }

    private void Start()
    {
        _currentEnergy = _maxEnergy;
    }

    public void GainEnergy(float amount)
    {
        _currentEnergy = Mathf.Min(_currentEnergy + amount, _maxEnergy);
    }

    public void ConsumeEnergy(float amount)
    {
        _currentEnergy = Mathf.Max(_currentEnergy - amount, 0);
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
    public void Damage(DamageInfo info)
    {
        _currentEnergy -= info.Amount;
        PlayerDamaged.Invoke();
    }

    private void Die()
    {
        Debug.Log("u ded lole");
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        _currentEnergy = Math.Max(0, _currentEnergy - _energyDrainPerSecond * Time.fixedDeltaTime);

        if (_currentEnergy == 0) 
        { 
            if(!_dyingFromNoEnergy)
            {
                _dyingFromNoEnergy = true;
                _timeToDie = Time.time + _zeroEnergyDeathTimerSeconds;
            }
            else
            {
                if (Time.time >= _timeToDie)
                {
                    Die();
                }
            }
        }
        else
        {
            _dyingFromNoEnergy = false;
        }

            _moltTimerSeconds -= Time.deltaTime;

        if (_moltTimerSeconds <= 0)
        {
            TryGrow();
            _moltTimerSeconds = _timeBeforeMolt;
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