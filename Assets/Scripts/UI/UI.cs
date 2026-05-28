using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private StarvingWarning _starveWarning;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private MoltWarning _moltWarning;

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
        //this will be done soon.
        //PlayerEnergy.PlayerStarving += _starveWarning.ToggleWarning;
        PlayerEnergy.PlayerMoltWarning += _moltWarning.TurnOnWarning;
        PlayerEnergy.Molted += _moltWarning.TurnOffWarning;
    }

    void OnDestroy()
    {
        //PlayerEnergy.PlayerStarving -= _starveWarning.ToggleWarning;
        PlayerEnergy.PlayerMoltWarning -= _moltWarning.TurnOnWarning;
        PlayerEnergy.Molted -= _moltWarning.TurnOffWarning;
    }

    internal void SetPauseMenu(bool paused)
    {
        _pauseMenu.SetActive(paused);
    }
}
