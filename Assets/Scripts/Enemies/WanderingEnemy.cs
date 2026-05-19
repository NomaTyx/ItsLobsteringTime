using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WanderingEnemy : EnemyController
{
    [Header("Stats")]
    [SerializeField] private float _wanderingSpeed = 5;
    [SerializeField] private float _combatSpeed = 2.5f;

    [Header("Food")]
    [SerializeField] private float _foodEatingRange = 5f;
    [SerializeField] private float _secondsBetweenBites = 1;
    [SerializeField] private float _chanceToEatFood = 0.25f;

    [Header("Combat")]
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _playerDetectionRadius = 10;
    private CharacterMovement _characterMovement;

    private Food _destinationFood = null;

    private CapsuleCollider _playerCollider;

    protected override void Init()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    /// <summary>
    /// An AI state for an enemy when it is searching for food. Right now,
    /// it just wanders from food to food until it finds one.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator SearchingState()
    {
        _characterMovement.Speed = _wanderingSpeed;

        while (true) 
        {
            if (_destinationFood == null)
            {
                try
                {
                    _destinationFood = FindClosestFood();
                }
                catch
                {
                    Debug.Log("food is null");
                }
            }
            else
            {
                _movement.MoveTo(_destinationFood.transform.position);

                if(Vector3.Distance(transform.position, _destinationFood.transform.position) < _foodEatingRange)
                {
                    ChangeState(EatingState());
                }
            }

            yield return null;
        }
    }

    private Food FindClosestFood()
    {
        Food closestFood = FoodManager.Instance.FoodInScene[0].GetComponent<Food>();
        float closestDistance = Mathf.Infinity;

        foreach(GameObject food in FoodManager.Instance.FoodInScene)
        {
            if ((food.transform.position - gameObject.transform.position).magnitude < closestDistance) {
                closestDistance = (food.transform.position - gameObject.transform.position).magnitude;
                closestFood = food.GetComponent<Food>();
            }
        }

        return closestFood;
    }

    protected IEnumerator EatingState()
    {
        float eatingTimer = 0;

        while(true)
        {
            eatingTimer += Time.deltaTime;
            if(eatingTimer > _secondsBetweenBites)
            {
                Eat();
                eatingTimer = 0;
            }

            if(_destinationFood == null)
            {
                ChangeState(SearchingState());
            }
            
            yield return null;
        }
    }

    private void Eat()
    {
        if((int)Random.Range(0, 1 / _chanceToEatFood) == 0)
        {
            _destinationFood.OnEaten();
        }
    }

    protected override IEnumerator CombatState()
    {
        _characterMovement.Speed = _combatSpeed;
        _playerCollider = _target.GetComponent<CapsuleCollider>();

        Debug.Log("target is " + _target.transform.parent.name);
        while(true)
        {
            if(InRange())
            {
                _characterMovement.Stop();
                _weapon.TryAttack(_target.GetComponent<IAttackable>());
            }
            else
            {
                _characterMovement.MoveTo(_target.transform.position);
            }
            yield return null;
        }
    }

    private bool InRange()
    {
        return Vector3.Distance(_weapon.transform.position, _playerCollider.ClosestPointOnBounds(_weapon.transform.position)) <= _weapon.AttackRange;
    }

    private void Update()
    {
        //this is a band-aid fix. i will graduate to a more complex state machine at some point after gday because i am already having problems.
        if (PlayerController.Instance == null) return;

        if(Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= _playerDetectionRadius)
        {
            if(_target == null)
            {
                _target = PlayerController.Instance.gameObject;
                Debug.Log(gameObject.name);
                ChangeState(CombatState());
            }
        }
        else if(_target != null)
        {
            _target = null;
            ChangeState(SearchingState());
        }
    }
}
