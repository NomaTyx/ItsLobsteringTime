using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action MoltTimerExpired;

    [SerializeField] private float _maxMoltTimerSeconds = 1f;

    private float _moltTimerSeconds;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists a GameManager singleton!");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _moltTimerSeconds = _maxMoltTimerSeconds;
    }

    private void FixedUpdate()
    {
        _moltTimerSeconds -= Time.deltaTime;
        Debug.Log("Being called");

        if (_moltTimerSeconds <= 0)
        {
            MoltTimerExpired.Invoke();
            _moltTimerSeconds = _maxMoltTimerSeconds;
            Debug.Log("invoked");
        }
    }
}
