using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -9.81f;

    [SerializeField] private float _eatRange = 5;
    
    private float _movementX;
    private float _movementZ;

    private float _groundCheckOffset = 0.1f;

    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        _movementX = movementVector.x;
        _movementZ = movementVector.y;
    }

    public void OnAttack()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, _eatRange))
        {
            Food food = c.GetComponent<Food>();

            if (food == null) continue;

            food.onEaten();
        }
    }

    private void FixedUpdate()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movementDir = (forward * _movementZ + right * _movementX) * Time.deltaTime;

        if (!CheckGrounded())
        {
            movementDir.y += _gravity * Time.deltaTime;
            Debug.Log("not grounded");
        }

        _controller.Move(movementDir * _moveSpeed);

        transform.LookAt(transform.position + forward);
    }

    private bool CheckGrounded()
    {
        Vector3 lobsterHalfExtents = new Vector3(_controller.radius, _controller.height / 2 + _groundCheckOffset, _controller.radius);
        return Physics.OverlapBox(_controller.center, lobsterHalfExtents).Length > 1;
    }
}
