using System.Collections;
using UnityEngine;

public class EnemyController : Controller
{
    
    private IEnumerator _currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //change this later, this is just so that i can check if it works
        ChangeState(AggressiveState());
    }

    private void ChangeState(IEnumerator newState)
    {
        // stop current state
        if (_currentState != null) StopCoroutine(_currentState);

        // assign new state and start
        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private IEnumerator PatrolState()
    {
        yield return null;
    }

    private IEnumerator AggressiveState()
    {
        while(true)
        {
            transform.LookAt(PlayerController.Instance.gameObject.transform);
            //Debug.Log("Attack!");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
