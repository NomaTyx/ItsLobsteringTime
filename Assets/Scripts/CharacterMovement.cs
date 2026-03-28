using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// RequireComponent forces theses components to exist on the same gameObject
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class CharacterMovement3D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _acceleration = 20f;
    [SerializeField] private float _turnSpeed = 10f;

    [Header("Jumping")]
    [SerializeField] private float _gravity = 20f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _airControl = 0.1f;

    [Header("Grounding")]
    [SerializeField][Tooltip("Radius of ground check sphere")] private float _groundCheckRadius = 0.25f;
    [SerializeField][Tooltip("Starting height of ground check sphere")] private float _groundCheckOffset = 0.5f;
    [SerializeField][Tooltip("Ground check sphere travel distance")] private float _groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    [Header("Components")]
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    // properties
    public bool IsGrounded { get; private set; } // get / private set allows for public READ but only PRIVATE WRITE, this property is read-only
    public Vector3 MoveInput => _moveInput;      // returns _moveInput value publicly, but doesn't allow modification (read-only)

    private Vector3 _moveInput;
    private Vector3 _lookDirection;
    private Vector3 _groundNormal;

    // OnValidate is called only in the editor, when component values change
    private void OnValidate()
    {
        // automatically assign any unassigned components
        if (_collider == null) _collider = GetComponent<CapsuleCollider>();
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if (_navMeshAgent == null) _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_rigidbody != null) _rigidbody.freezeRotation = true;
    }

    private void Awake()
    {
        // match look direction with current character facing
        _lookDirection = transform.forward;

        // configure NavMeshAgent
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    // set desired move input direction
    public void SetMoveInput(Vector3 moveInput)
    {
        _moveInput = moveInput;
    }

    // set desired look position
    public void SetLookPosition(Vector3 aimPosition)
    {
        // find direction from position
        Vector3 lookDirection = (aimPosition - transform.position).normalized;
        SetLookDirection(lookDirection);
    }

    public void Stop()
    {
        // clear existing path
        _navMeshAgent.ResetPath();
        // stop manual movement
        SetMoveInput(Vector3.zero);
    }

    // attempt jump (stopped if airborne)
    public void Jump()
    {
        // stop if not grounded
        if (!IsGrounded) return;

        // calculate jump speed
        float jumpSpeed = Mathf.Sqrt(2f * _gravity * _jumpHeight);

        // calculate jump velocity
        Vector3 velocity = _rigidbody.linearVelocity;
        velocity.y = jumpSpeed;
        _rigidbody.linearVelocity = velocity;
    }

    // set desired look direction
    public void SetLookDirection(Vector3 lookDirection)
    {
        // throw out direction if too small, return stops function immediately
        if (lookDirection.magnitude < 0.01f) return;

        // flatten and normalize direction
        lookDirection.y = 0f;
        lookDirection.Normalize();

        _lookDirection = lookDirection;
    }

    // navigate to destination position
    public void MoveTo(Vector3 position)
    {
        // set NavMeshAgent destination
        _navMeshAgent.SetDestination(position);
    }

    private void FixedUpdate()
    {
        // check for ground
        IsGrounded = CheckGrounded();

        // check if NavMeshAgent has a path
        if (_navMeshAgent.hasPath)
        {
            // get current and desired position
            Vector3 current = transform.position;
            Vector3 next = _navMeshAgent.path.corners[1];
            Vector3 direction = (next - current).normalized;
            // override move and look inputs
            SetMoveInput(direction);
            SetLookDirection(direction);
        }

        // sync NavMeshAgent with character position
        _navMeshAgent.nextPosition = transform.position;

        // calculate velocity differential
        Vector3 currentVelocity = _rigidbody.linearVelocity;
        Vector3 targetVelocity = _moveInput * _runSpeed;
        Vector3 velocityDifferential = targetVelocity - currentVelocity;
        // zero out Y, we don't want to accelerate up/down
        velocityDifferential.y = 0f;

        // find air control modifier
        // option 1:
        //float airModifier = IsGrounded ? 1f : _airControl;
        // option 2:
        float airModifier = 1f;
        if (!IsGrounded) airModifier = _airControl;

        // calculate force required to reach target
        Vector3 moveForce = velocityDifferential * _acceleration * airModifier;

        // add gravity
        moveForce -= _groundNormal * _gravity;

        // apply acceleration force
        _rigidbody.AddForce(moveForce);

        // rotate character towards aim direction
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(_lookDirection);    // LookRotation turns a direction into a rotation
        Quaternion rotation = Quaternion.Slerp(currentRotation, targetRotation, _turnSpeed * Time.fixedDeltaTime);
        transform.rotation = rotation;
    }

    private bool CheckGrounded()
    {
        // spherecast inputs
        Vector3 start = transform.position + transform.up * _groundCheckDistance;
        float radius = _groundCheckRadius;
        Vector3 direction = -transform.up;
        float distance = _groundCheckDistance;
        LayerMask mask = _groundMask;

        // perform spherecast
        if (Physics.SphereCast(start, radius, direction, out RaycastHit hit, distance, mask))
        {
            // we hit something
            _groundNormal = hit.normal;
            return true;
        }

        // nothing was hit, assume not grounded
        _groundNormal = transform.up;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // draw starting position
        Vector3 start = transform.position + transform.up * _groundCheckOffset;
        Gizmos.DrawWireSphere(start, _groundCheckRadius);

        // draw ending position
        Vector3 end = start - transform.up * _groundCheckDistance;
        Gizmos.DrawWireSphere(end, _groundCheckRadius);
    }
}