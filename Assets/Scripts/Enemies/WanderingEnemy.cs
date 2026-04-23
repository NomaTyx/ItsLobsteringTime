using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WanderingEnemy : EnemyController
{
    [Header("Speed")]
    [SerializeField] private float _wanderingSpeed = 5;
    [SerializeField] private float _combatSpeed = 2.5f;

    [Header("Combat")]
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _playerDetectionRadius = 10;
    private CharacterMovement _characterMovement;

    private Food _destinationFood = null;

    private SphereCollider _playerDetection;

    protected override void Init()
    {
        _playerDetection = GetComponent<SphereCollider>();
        _playerDetection.radius = _playerDetectionRadius;
        _characterMovement = GetComponent<CharacterMovement>();
    }

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
                catch (Exception e)
                {
                    Debug.Log("food is null");
                }
            }
            else
            {
                _movement.MoveTo(_destinationFood.transform.position);
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

    private void OnTriggerEnter(Collider other)
    {
        _target = other.gameObject;
        ChangeState(CombatState());
    }

    protected override IEnumerator CombatState()
    {
        _characterMovement.Speed = _combatSpeed;
        while(true)
        {
            _characterMovement.MoveTo(_target.transform.position);
            if(Vector3.Distance(transform.position, _target.transform.position) <= _weapon.AttackRange)
            {
                _weapon.TryAttackWithTarget(_target.GetComponent<IAttackable>());
            }
            yield return null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _target = null;
        ChangeState(SearchingState());
    }
}
