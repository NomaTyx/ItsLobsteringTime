using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Health))]
public abstract class EnemyController : Controller
{
    protected CharacterMovement _movement;
    protected IEnumerator _currentState;
    protected Health _health;

    protected void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _health = GetComponent<Health>();

        _health.Died += Die;
        Init();
    }

    private void OnDestroy()
    {
        _health.Died -= Die;
        EnemyManager.Instance.RemoveEnemyFromScene(gameObject);
    }

    protected abstract void Init();

    void Start()
    {
        ChangeState(SearchingState());
    }

    public void Die()
    {
        ChangeState(DeadState());
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

    protected virtual IEnumerator DeadState()
    {
        _movement.Stop();
        Destroy(gameObject);
        yield return null;
    }
}
