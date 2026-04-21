using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public abstract class EnemyController : Controller
{
    private IEnumerator _currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //change this later, this is just so that i can check if it works
        ChangeState(PatrolState());
    }

    private void ChangeState(IEnumerator newState)
    {
        // stop current state
        if (_currentState != null) StopCoroutine(_currentState);

        // assign new state and start
        _currentState = newState;
        StartCoroutine(_currentState);
    }

    protected virtual IEnumerator PatrolState()
    {
        Debug.Log("hell on earth");
        yield return null;
    }

    protected virtual IEnumerator AggressiveState()
    {
        while(true)
        {
            transform.LookAt(PlayerController.Instance.gameObject.transform);
            //Debug.Log("Attack!");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
