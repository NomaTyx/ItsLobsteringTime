using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Health))]
public abstract class EnemyController : Controller
{
    public int Size => _size;
    protected CharacterMovement _movement;
    protected IEnumerator _currentState;
    protected Health _health;
    private Action _onPlayerDead;
    protected int _size = 1;

    protected void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _health = GetComponent<Health>();

        _health.Died += Die;
        _onPlayerDead = () => ChangeState(PlayerDeadState());
        PlayerEnergy.PlayerDead += _onPlayerDead;
        Init();
    }

    private void OnDestroy()
    {
        _health.Died -= Die;
        if (_onPlayerDead != null) PlayerEnergy.PlayerDead -= _onPlayerDead;
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

    protected virtual IEnumerator PlayerDeadState()
    {
        while (true)
        {
            _movement.Stop();
            transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
            yield return null;
        }
    }

    public void SetSize(int size)
    {
        if (size < 1)
        {
            Debug.Log("size was clamped to 1");
            size = 1;
        }
        transform.localScale = Vector3.one * Mathf.Pow(1.15f, size - 1);
    }
}
