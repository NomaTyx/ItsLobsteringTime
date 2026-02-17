using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _eatRange = 5;

    public static PlayerController Instance;
    
    private float _movementX;
    private float _movementZ;


    private Rigidbody _rb;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There already exists a PlayerController instance");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        _movementX = movementVector.x;
        _movementZ = movementVector.y;
    }

    public void OnAttack()
    {
        //TODO: Implement cooldown????
        foreach (Collider c in Physics.OverlapSphere(transform.position, _eatRange))
        {
            Food food = c.GetComponent<Food>();

            if (food == null) continue;

            food.OnEaten();
            break;
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

        Vector3 movementDir = forward * _movementZ + right * _movementX;

        //if i do somethign like a knockback effect on the player, i will need some sort of boolean to disable 
        _rb.linearVelocity = movementDir * _moveSpeed;

        transform.LookAt(transform.position + forward);
    }
}
