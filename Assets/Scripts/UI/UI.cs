using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private StarvingWarning _starveWarning;
    [SerializeField] private GameObject _pauseMenu;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists a UI singleton");
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    internal void SetPauseMenu(bool paused)
    {
        _pauseMenu.SetActive(paused);
    }
}
