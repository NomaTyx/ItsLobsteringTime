using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private PlayerMovement _movement;
    private Claw _claw;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists a PlayerController instance");
            return;
        }

        Instance = this;
        _movement = GetComponent<PlayerMovement>();
        _claw = GetComponent<Claw>();
    }

    public void OnMove(InputValue movementValue)
    {
        _movement.UpdateMoveInput(movementValue.Get<Vector2>());
    }

    public void OnAttack()
    {
        _claw.TryAttack();
    }
}
