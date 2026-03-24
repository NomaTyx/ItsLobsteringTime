using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.Splines;
using Unity.Mathematics;

    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMovement : MonoBehaviour
    {
         // private serialized fields
        [field: Header("Movement")]
        [field: SerializeField] public float Speed { get; set; } = 5f;
        [field: SerializeField] public float Acceleration { get; set; } = 10f;
        [field: SerializeField] public float TurnSpeed { get; set; } = 15f;
        [field: SerializeField] public bool OnlyTurnWithInput { get; set; } = true;
        [field: SerializeField] public float StoppingDistance { get; set; } = 0.25f;
        [field: SerializeField] public bool LookInMoveDirection { get; set; } = true;
        [field: SerializeField] public bool ControlRotation { get; set; } = true;       // character turns towards movement direction
        [field: SerializeField] public bool Fix3DSpriteRotation { get; set; } = false;
        [field: SerializeField] public bool ParentToSurface { get; set; } = false;

        [field: Header("Airborne")]
        [field: SerializeField] public float Gravity { get; set; } = -20f;             // custom gravity value
        [field: SerializeField] public float AirControl { get; set; } = 0.1f;          // percentage of acceleration applied while airborne
        [field: SerializeField] public bool AirTurning { get; set; } = true;           // character can turn while airborne

        [field: Header("Size")]
        [field: SerializeField] public float Height { get; protected set; } = 0.3f;
        [field: SerializeField] public float Radius { get; protected set; } = 0.75f;

        [field: Header("Grounding")]
        [field: SerializeField] protected float GroundCheckOffset { get; set; } = 0.1f;         // height inside character where grounding ray starts
        [field: SerializeField] protected float GroundCheckDistance { get; set; } = 0.4f;       // distance down from offset position
        [field: SerializeField] protected float MaxSlopeAngle { get; set; } = 40f;              // maximum climbable slope, character will slip on anything higher
        [field: SerializeField] protected LayerMask GroundMask { get; set; } = 1 << 0;          // mask for layers considered the ground

        [field: Header("Events")]
        [field: SerializeField] protected float MinGroundedVelocity { get; set; } = 5f;
        public event Action<GameObject> OnGrounded;
        public event Action<GameObject> OnFootstep;

        // public properties
        public float MoveSpeedMultiplier { get; set; } = 1f;
        public float LastGroundedDistance => Vector3.Distance(transform.position, LastGroundedPosition);

        // public-get protected-set properties
        public Vector3 Velocity { get => Rigidbody.linearVelocity; protected set => Rigidbody.linearVelocity = value; }
        public Vector3 FlattenedVelocity => new Vector3(Velocity.x, 0f, Velocity.z);
        public float NormalizedSpeed => FlattenedVelocity.magnitude / Speed;
        public Vector3 MoveInput { get; protected set; }
        public Vector3 LocalMoveInput { get; protected set; }
        public Vector3 LookDirection { get; protected set; }
        public bool HasMoveInput { get; protected set; }
        public bool HasTurnInput { get; protected set; }
        public bool IsGrounded { get; protected set; }
        public GameObject SurfaceObject { get; protected set; }
        public Vector3 SurfaceVelocity { get; protected set; }
        public bool CanMove { get; set; } = true;
        public bool CanTurn { get; set; } = true;
        public Vector3 GroundNormal { get; protected set; } = Vector3.up;
        public float LastGroundedTime { get; protected set; }
        public Vector3 LastGroundedPosition { get; protected set; }

        // useful properties
        public float TurnSpeedMultiplier { get; set; } = 1f;
        protected Vector3 GroundCheckStart => transform.position + transform.up * GroundCheckOffset;
        public Vector3 SplineLookDirection { get; protected set; }
        public bool HasPath => NavMeshAgent.hasPath;
        public bool HasCompletePath => NavMeshAgent.hasPath && Vector3.Distance(NavMeshAgent.path.corners[NavMeshAgent.path.corners.Length - 1], NavMeshAgent.destination) < StoppingDistance;

        // step height fields
        [field: Header("Step Height")]
        [field: SerializeField] protected float StepHeight { get; set; } = 0.3f;
        [field: SerializeField] protected float StepHeightAllowance { get; set; } = 0.1f;
        [field: SerializeField] protected float StepHeightForwardOffset { get; set; } = 0.05f;

        [field: Header("Spline Contstraint")]
        [field: SerializeField] public bool EnableSplineConstraint { get; set; }
        [field: SerializeField] public SplineContainer SplineContainer { get; set; }
        [field: SerializeField] public float SplineGravitation { get; set; } = 20f;

        [field: Header("Components")]
        [field: SerializeField] protected Rigidbody Rigidbody { get; set; }
        [field: SerializeField] protected NavMeshAgent NavMeshAgent { get; set; }
        [field: SerializeField] protected CapsuleCollider CapsuleCollider { get; set; }

        protected virtual void Reset()
        {
            Rigidbody = GetComponent<Rigidbody>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            CapsuleCollider = GetComponent<CapsuleCollider>();
        }

        protected virtual void Awake()
        {
            // assign frictionless physic material
            CapsuleCollider.material = new PhysicsMaterial("NoFriction") { staticFriction = 0f, dynamicFriction = 0f, frictionCombine = PhysicsMaterialCombine.Minimum };

            // disable NavMeshAgent movement
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;
            NavMeshAgent.stoppingDistance = 0.1f;

            // configure physics
            Rigidbody.freezeRotation = true;
            Rigidbody.useGravity = false;
            Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            Rigidbody.isKinematic = false;

            // match look direction to current facing
            LookDirection = transform.forward;
        }

        public void SetMoveInput(Vector3 input)
        {
            if (!CanMove)
            {
                MoveInput = Vector3.zero;
                return;
            }

            input = Vector3.ClampMagnitude(input, 1f);
            // set input to 0 if small incoming value
            HasMoveInput = input.magnitude > 0.1f;
            input = HasMoveInput ? input : Vector3.zero;
            // remove y component of movement but retain overall magnitude
            Vector3 flattened = new Vector3(input.x, 0f, input.z);
            flattened = flattened.normalized * input.magnitude;
            MoveInput = flattened;
            // finds movement input as local direction rather than world direction
            LocalMoveInput = transform.InverseTransformDirection(MoveInput);
        }

        // sets character look direction, flattening y-value
        public void SetLookDirection(Vector3 direction)
        {
            if (!CanTurn || direction.magnitude < 0.1f)
            {
                HasTurnInput = false;
                return;
            }
            HasTurnInput = true;
            LookDirection = new Vector3(direction.x, 0f, direction.z).normalized;
        }

        public void SetLookPosition(Vector3 position)
        {
            Vector3 direction = Vector3.ClampMagnitude(position - transform.position, 1f);
            SetLookDirection(direction);
        }

        // path to destination using navmesh
        public virtual void MoveTo(Vector3 destination)
        {
            if (!NavMeshAgent.isActiveAndEnabled || !NavMeshAgent.isOnNavMesh) return;
            NavMeshAgent.SetDestination(destination);
        }

        // stop all movement
        public virtual void Stop()
        {
            SetMoveInput(Vector3.zero);
            if (!NavMeshAgent.isActiveAndEnabled || !NavMeshAgent.isOnNavMesh) return;
            NavMeshAgent.ResetPath();
        }

        protected virtual void FixedUpdate()
        {
            // check for the ground
            IsGrounded = CheckGrounded();

            // overrides current input with pathing direction if MoveTo has been called
            if (NavMeshAgent.hasPath && NavMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid && NavMeshAgent.path.corners.Length >= 2)
            {
                Vector3 nextPathPoint = NavMeshAgent.steeringTarget;
                Vector3 lastPathPoint = NavMeshAgent.path.corners[NavMeshAgent.path.corners.Length - 1];
                float nextPointDistance = Vector3.Distance(nextPathPoint, transform.position);
                if (nextPointDistance < StoppingDistance) NavMeshAgent.SetDestination(NavMeshAgent.destination);
                float lastPointDistance = Vector3.Distance(lastPathPoint, transform.position);
                bool pathEndReached = lastPointDistance < StoppingDistance;
                Vector3 pathDir = (nextPathPoint - transform.position).normalized;
 
                SetMoveInput(pathDir);
                if (LookInMoveDirection) SetLookDirection(pathDir);

                bool destinationReached = Vector3.Distance(NavMeshAgent.destination, transform.position) < StoppingDistance;
                // stop off destination reached
                if (pathEndReached || (StoppingDistance > 0f && destinationReached))
                {
                    SetLookPosition(NavMeshAgent.destination);
                    Stop();
                }
            }

            // syncs navmeshagent position with character position
            if(NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.nextPosition = transform.position;
            }

            // find flattened movement vector based on ground normal
            Vector3 input = MoveInput;
            Vector3 right = Vector3.Cross(transform.up, input);
            Vector3 forward = Vector3.Cross(right, GroundNormal);

            // move character along spline
            if (EnableSplineConstraint && SplineContainer != null)
            {
                // spline closest point and tangent
                Spline spline = SplineContainer.Spline;
                Vector3 splineRelativePosition = SplineContainer.transform.InverseTransformPoint(transform.position);
                SplineUtility.GetNearestPoint(spline, splineRelativePosition, out float3 nearest, out float t);
                Vector3 splineWorldPosition = SplineContainer.transform.TransformPoint(nearest);
                Vector3 splineTangent = SplineUtility.EvaluateTangent(spline, t);
                splineTangent.y = 0f;
                splineTangent.Normalize();

                // float direction to closest point
                Vector3 dirToSplineCenter = splineWorldPosition - transform.position;
                dirToSplineCenter.y = 0f;
                float splineFlatDistance = dirToSplineCenter.magnitude;
                dirToSplineCenter.Normalize();

                // force bringing character back to spline center
                float gravitationDot = Vector3.Dot(splineTangent, dirToSplineCenter);
                float gravitationCorrection = 1f - Math.Abs(gravitationDot);
                float sideInput = Vector3.Dot(MoveInput, splineTangent);
                Rigidbody.AddForce(gravitationCorrection * Mathf.Clamp01(splineFlatDistance) * SplineGravitation * dirToSplineCenter);

                // correct movement direction along spline
                forward = MoveInput.magnitude * sideInput * splineTangent;
                SplineLookDirection = splineTangent * Mathf.Sign(sideInput);
            }

            float speed = Speed;
            // calculates desirection movement velocity
            Vector3 targetVelocity = forward * (speed * MoveSpeedMultiplier);
            if (!CanMove) targetVelocity = Vector3.zero;
            // adds velocity of surface under character, if character is stationary
            targetVelocity += SurfaceVelocity * (1f - Mathf.Abs(MoveInput.magnitude));
            // calculates acceleration required to reach desired velocity and applies air control if not grounded
            Vector3 velocityDiff = targetVelocity - Velocity;
            velocityDiff.y = 0f;
            float control = IsGrounded ? 1f : AirControl;
            Vector3 acceleration = velocityDiff * (Acceleration * control);
            // zeros acceleration if airborne and not trying to move (allows for nice jumping arcs)
            if (!IsGrounded && !HasMoveInput) acceleration = Vector3.zero;
            // add gravity
            acceleration += GroundNormal * Gravity;

            Rigidbody.AddForce(acceleration * Rigidbody.mass);

            StepCheck();
        }

        protected virtual void Update()
        {
            // rotates character towards movement direction
            if (ControlRotation && (HasTurnInput || !OnlyTurnWithInput) && (IsGrounded || AirTurning))
            {
                Quaternion rotation = Rigidbody.rotation;
                if(!Fix3DSpriteRotation)
                {
                    if (EnableSplineConstraint && HasMoveInput) LookDirection = SplineLookDirection;
                    Quaternion targetRotation = Quaternion.LookRotation(LookDirection);
                    rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * TurnSpeedMultiplier * Time.deltaTime);
                }   // rotate sprite character properly
                else if (Fix3DSpriteRotation && Mathf.Abs(MoveInput.x) > 0.2f)
                {
                    float spriteAngle = LookDirection.x > 0 ? 0f : 180f;
                    rotation = Quaternion.Euler(0f, spriteAngle, 0f);
                }
                Rigidbody.MoveRotation(rotation);
                transform.rotation = rotation;
            }
        }

        protected virtual bool CheckGrounded()
        {
            // raycast to find ground
            bool hit = Physics.Raycast(GroundCheckStart, -transform.up, out RaycastHit hitInfo, GroundCheckDistance, GroundMask);

            // set default ground surface normal and SurfaceVelocity
            GroundNormal = Vector3.up;
            SurfaceVelocity = Vector3.zero;

            // if ground wasn't hit, character is not grounded
            if (!hit) return false;

            // gets velocity of surface underneath character if applicable
            if (hitInfo.rigidbody != null) SurfaceVelocity = hitInfo.rigidbody.linearVelocity;

            // test angle between character up and ground, angles above _maxSlopeAngle are invalid
            bool angleValid = Vector3.Angle(transform.up, hitInfo.normal) < MaxSlopeAngle;
            if (angleValid)
            {
                // record last time character was grounded and set correct floor normal direction
                LastGroundedTime = Time.timeSinceLevelLoad;
                GroundNormal = hitInfo.normal;
                LastGroundedPosition = transform.position;
                SurfaceObject = hitInfo.collider.gameObject;
                if(ParentToSurface) transform.SetParent(SurfaceObject.transform);
                return true;
            }

            SurfaceObject = null;
            if(ParentToSurface) transform.SetParent(null);
            return false;
        }

        // check for step in front of player and bump up to that height
        protected void StepCheck()
        {
            if(!IsGrounded) return;

            Vector3 moveInputRight = Vector3.Cross(transform.up, MoveInput.normalized);
            Vector3 groundNormalForward = Vector3.Cross(-GroundNormal, moveInputRight);
            Ray blockingRay = new Ray(transform.position + transform.up * StepHeightAllowance, groundNormalForward);
            float blockingDistance = Radius + StepHeightForwardOffset;
            bool blockingHit = Physics.Raycast(blockingRay, blockingDistance, GroundMask);
            if (!blockingHit) return;

            Vector3 stepHeightPosition = MoveInput.normalized * (StepHeightForwardOffset + Radius) + transform.up * StepHeight + transform.position;
            Ray stepRay = new Ray(stepHeightPosition, -transform.up);
            float distance = StepHeight - StepHeightAllowance;
            bool stepHit = Physics.Raycast(stepRay, out RaycastHit stepHitInfo, distance, GroundMask);
            float groundNormalAngle = Vector3.Angle(GroundNormal, stepHitInfo.normal);
            if(!stepHit) return;

            float stepOffset = stepHitInfo.point.y - transform.position.y;
            float stepVelocity = Mathf.Sqrt(2f * -Gravity * stepOffset);
            Velocity = new Vector3(Velocity.x, stepVelocity, Velocity.z);
        }

        // check for landing on the ground
        protected virtual void OnCollisionEnter(Collision collision)
        {
            float landingCollisionMaxDistance = 0.25f;
            Vector3 point = collision.contacts[0].point;
            if(Mathf.Abs(collision.relativeVelocity.y) < MinGroundedVelocity) return;
            if(Vector3.Distance(point, transform.position) < landingCollisionMaxDistance)
            {
                OnGrounded.Invoke(collision.gameObject);
            }
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawRay(GroundCheckStart, -transform.up * GroundCheckDistance);

            // step height debug
            Gizmos.color = Color.cyan;
            Vector3 stepHeightPosition = MoveInput.normalized * (StepHeightForwardOffset + Radius) + transform.up * StepHeight + transform.position;
            Ray stepRay = new Ray(stepHeightPosition, -transform.up);
            float distance = StepHeight - StepHeightAllowance;
            Gizmos.DrawRay(stepRay.origin, stepRay.direction * distance);
        }
    }