using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    public static PlayerController Instance;

    private CharacterMovement _movement;
    private Claw _claw;

    private Vector2 MoveInput;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists a PlayerController instance");
            return;
        }

        Instance = this;
        //_movement = GetComponent<PlayerMovement>();
        _movement = GetComponent<CharacterMovement>();
        _claw = GetComponent<Claw>();
    }

    public void OnMove(InputValue movementValue)
    {
        MoveInput = movementValue.Get<Vector2>();
    }

    public void OnAttack()
    {
        _claw.TryAttack();
    }

    public void OnEat()
    {
        PlayerEnergy.Instance.Eat();
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
}