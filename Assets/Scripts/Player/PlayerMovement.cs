using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private float _movementX;
    private float _movementZ;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void UpdateMoveInput(Vector2 movementVector)
    {
        _movementX = movementVector.x;
        _movementZ = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movementDir = forward * _movementZ + right * _movementX;

        //if i do somethign like a knockback effect on the player, i will need some sort of boolean to disable 
        _rb.linearVelocity = movementDir * _moveSpeed;

        transform.LookAt(transform.position + forward);
    }
}
