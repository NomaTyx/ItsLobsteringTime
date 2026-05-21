using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    public float DashEnergyCost => _dashEnergyCost;
    public static PlayerController Instance;

    [SerializeField] private float _dashEnergyCost = 15f;
    
    private CharacterMovement _movement;
    private Weapon _weapon;
    private Vector2 MoveInput;
    private Animator _animator;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists a PlayerController instance");
            return;
        }

        Instance = this;
        _movement = GetComponent<CharacterMovement>();
        _weapon = GetComponentInChildren<Weapon>();
        _animator = GetComponentInChildren<Animator>();
        _animator.runtimeAnimatorController = _weapon.AnimController;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue movementValue)
    {
        MoveInput = movementValue.Get<Vector2>();
    }

    public void OnAttack()
    {
        _weapon.TryAttack();
    }

    public void OnEat()
    {
        PlayerEnergy.Instance.Eat();
    }

    public void OnDash()
    {
        if (PlayerEnergy.Instance.Energy <= 0) return;

        _movement.Dash();
        PlayerEnergy.Instance.ConsumeEnergy(_dashEnergyCost);
    }

    protected virtual void Update()
    {
        if (_movement == null) return;

        // find correct right/forward directions based on main camera rotation
        Vector3 up = Vector3.up;
        Vector3 right = Camera.main.transform.right;
        Vector3 forward = Vector3.Cross(right, up);
        Vector3 moveInput = forward * MoveInput.y + right * MoveInput.x;

        // send player input to character movement
        _movement.SetMoveInput(moveInput);
        _movement.SetLookDirection(moveInput);
        _movement.SetLookDirection(Camera.main.transform.forward);
    }

    private void OnPause()
    {
        GameManager.Instance.PauseGame();
    }
}