using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private StarvingWarning _starveWarning;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private MoltWarning _moltWarning;
    [SerializeField] private TextMeshProUGUI _deadText;
    [SerializeField] private HurtOverlay _hurtOverlay;

    private WaitForSeconds _hurtOverlayWait;
    
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
        PlayerEnergy.PlayerDead += OnPlayerDead;
        PlayerEnergy.PlayerDamaged += OnPlayerDamaged;
    }

    void OnDestroy()
    {
        //PlayerEnergy.PlayerStarving -= _starveWarning.ToggleWarning;
        PlayerEnergy.PlayerMoltWarning -= _moltWarning.TurnOnWarning;
        PlayerEnergy.Molted -= _moltWarning.TurnOffWarning;
        PlayerEnergy.PlayerDead -= OnPlayerDead;
        PlayerEnergy.PlayerDamaged -= OnPlayerDamaged;
    }

    public void SetPauseMenu(bool paused)
    {
        _pauseMenu.SetActive(paused);
    }

    private void OnPlayerDead(DeathCause cause)
    {
        _moltWarning.TurnOffWarning();
        switch (cause)
        {
            case DeathCause.Starvation:
                _deadText.text = "You starved to death";
                break;
            case DeathCause.Enemy:
                _deadText.text = "You got killed and eaten";
                break;
            case DeathCause.Molting:
                _deadText.text = "You got stuck in your old shell and suffocated";
                break;
            default:
                _deadText.text = "i have no earthly idea how you died";
                break;
        }
        _deadText.gameObject.SetActive(true);
    }

    private void OnPlayerDamaged()
    {
        _hurtOverlay.Display();
    }
}
