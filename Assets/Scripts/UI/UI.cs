using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private StarvingWarning _starveWarning;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private MoltWarning _moltWarning;
    [SerializeField] private GameObject _deadText;

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
        PlayerEnergy.PlayerDead += _moltWarning.TurnOffWarning;
        PlayerEnergy.PlayerDead += SetDeadText;
    }

    void OnDestroy()
    {
        //PlayerEnergy.PlayerStarving -= _starveWarning.ToggleWarning;
        PlayerEnergy.PlayerMoltWarning -= _moltWarning.TurnOnWarning;
        PlayerEnergy.Molted -= _moltWarning.TurnOffWarning;
        PlayerEnergy.PlayerDead -= _moltWarning.TurnOffWarning;
        PlayerEnergy.PlayerDead += SetDeadText;
    }

    public void SetPauseMenu(bool paused)
    {
        _pauseMenu.SetActive(paused);
    }

    private void SetDeadText()
    {
        _deadText.SetActive(true);
    }
}
