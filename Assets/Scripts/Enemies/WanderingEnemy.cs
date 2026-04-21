using System;
using System.Collections;
using UnityEngine;

public class WanderingEnemy : EnemyController
{
    Food destinationFood = null;
    protected override IEnumerator PatrolState()
    {
        while (true) 
        {
            Debug.Log("override is being called");
            if (destinationFood == null)
            {
                Debug.Log("null");
                try
                {
                    destinationFood = FindClosestFood();
                }
                catch (Exception e)
                {
                    Debug.Log("food is null");
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
}
