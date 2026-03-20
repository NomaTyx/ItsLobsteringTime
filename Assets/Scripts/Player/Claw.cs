using UnityEngine;

public class Claw : MonoBehaviour
{
    [SerializeField] private float _eatRange = 5;

    public virtual void TryAttack()
    {
        //TODO: add separate logic for enemies vs 
        Food closestTarget = null;
        float closestDistance = Mathf.Infinity;

        //TODO: Implement cooldown????
        foreach (Collider c in Physics.OverlapSphere(transform.position, _eatRange))
        {
            Food target = c.GetComponent<Food>();
            
            if (target == null) continue;

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < closestDistance)
            {
                closestTarget = target;
                closestDistance = distanceToTarget;
            }
        }

        if (closestTarget != null) 
        {
            if(closestTarget.GetType() == typeof(Food))
            {
                Eat((Food) closestTarget);
            }
            /*else if(closestTarget.GetType() == typeof(Enemy)) {
                Attack((Enemy) closestTarget);
            }*/
        }
        
    }

    public virtual void Eat(Food food)
    {
        food.OnEaten();
    }
}
