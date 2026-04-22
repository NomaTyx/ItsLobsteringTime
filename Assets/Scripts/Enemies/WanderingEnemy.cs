using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WanderingEnemy : EnemyController
{
    [SerializeField] private float _playerDetectionRadius = 10;

    private Food _destinationFood = null;

    private SphereCollider _playerDetection;

    protected override void Init()
    {
        _playerDetection = GetComponent<SphereCollider>();
        _playerDetection.radius = _playerDetectionRadius;
    }

    protected override IEnumerator SearchingState()
    {
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
        ChangeState(CombatState());
    }

    private void OnTriggerExit(Collider other)
    {
        ChangeState(SearchingState());
    }
}
