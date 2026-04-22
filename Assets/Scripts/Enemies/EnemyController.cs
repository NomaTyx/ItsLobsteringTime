using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public abstract class EnemyController : Controller
{
    protected CharacterMovement _movement;
    private IEnumerator _currentState;

    void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
    }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //change this later, this is just so that i can check if it works
        ChangeState(SearchingState());
    }

    protected void ChangeState(IEnumerator newState)
    {
        // stop current state
        if (_currentState != null) StopCoroutine(_currentState);

        // assign new state and start
        _currentState = newState;
        StartCoroutine(_currentState);
    }

    protected virtual IEnumerator SearchingState()
    {
        Debug.Log("base searching state");
        yield return null;
    }

    protected virtual IEnumerator CombatState()
    {
        Debug.Log("Base attack");
        yield return null;
    }

    protected virtual IEnumerator EatingState()
    {
        Debug.Log("Base eat");
        yield return null;
    }
}
