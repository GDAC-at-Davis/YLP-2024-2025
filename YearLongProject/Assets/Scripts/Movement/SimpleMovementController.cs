using Animancer;
using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace Movement
{
    public class SimpleMovementController : MonoBehaviour
    {
        [BoldHeader("Simple Movement")]
        [InfoBox("Modify the character's basic movement stats here")]
        [Header("Depends")]

        [SerializeField]
        private CharacterRigidbody2D characterRigidbody;


        [Header("Ground")]

        [SerializeField]
        private float groundMaxSpeed;

        [SerializeField]
        private float groundAcceleration;

        [Header("Air")]

        [SerializeField]
        private float airMaxSpeed;

        [SerializeField]
        private float airAcceleration;

        [Header("Detection")]

        [SerializeField]
        private LayerMask groundLayer;

        [SerializeField]
        private float groundCheckDistance;
	
	[Header("Horizontal Movement Deadzone")]
	[SerializeField] 
	private float deadzoneValue;

        [Header("Events")]

        [SerializeField]
        private UnityEvent onTouchGround;

        [SerializeField]
        private UnityEvent onLeaveGround;

        
	private Vector2 Position => characterRigidbody ? characterRigidbody.Position : Vector2.zero;

        private bool inJump;
        private bool isGrounded;
        private bool wasGrounded;
        private float horizontalInput;
        private float jumpVelocity;

        private void FixedUpdate()
        {
            // Grounded logic
            isGrounded = Physics2D.Raycast(Position, -Vector2.up, groundCheckDistance, groundLayer);

            if (isGrounded && !wasGrounded)
            {
                onTouchGround?.Invoke();
            }
            else if (!isGrounded && wasGrounded)
            {
                onLeaveGround?.Invoke();
            }

            wasGrounded = isGrounded;

            // Horizontal Movement logic
            float speed = isGrounded ? groundMaxSpeed : airMaxSpeed;
            float acceleration = isGrounded ? groundAcceleration : airAcceleration;

            float playerIntendedMove = horizontalInput * speed;
            float newVelocity = Mathf.Lerp(characterRigidbody.LinearVelocity.x, playerIntendedMove,
                acceleration * Time.fixedDeltaTime);
            SetHorizontalVelocity(newVelocity);

            // Jump logic
            if (inJump)
            {
                SetVerticalVelocity(jumpVelocity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Position, Position - Vector2.up * groundCheckDistance);
        }

        public void SetHorizontalInput(float desiredMove)
        {
            //if (desiredMove == 0)
	    if (Mathf.Abs(desiredMove) <= deadzoneValue)
            {
                horizontalInput = 0;
            }
            else
            {
                horizontalInput = desiredMove > 0 ? 1 : -1;
            }
        }

        public void SetJumpVelocity(float desiredJumpVelocity)
        {
            jumpVelocity = desiredJumpVelocity;
        }

        public void StartJump()
        {
            inJump = true;
        }

        public void StopJump()
        {
            inJump = false;
        }

        public void AddVelocity(Vector2 velocity)
        {
            characterRigidbody.LinearVelocity += velocity;
        }

        public void SetVelocity(Vector2 velocity)
        {
            characterRigidbody.LinearVelocity = velocity;
        }

        public void SetHorizontalVelocity(float velocity)
        {
            Vector3 curVel = characterRigidbody.LinearVelocity;
            characterRigidbody.LinearVelocity = new Vector2(velocity, curVel.y);
        }

        public void SetVerticalVelocity(float velocity)
        {
            Vector3 curVel = characterRigidbody.LinearVelocity;
            characterRigidbody.LinearVelocity = new Vector2(curVel.x, velocity);
        }

        public void ApplyImpulseForce(Vector2 force)
        {
            characterRigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        public bool GetIsGrounded()
        {
            return isGrounded;
        }

        public void SetAllowMovement(bool isAllowed)
        {
        }
    }
}
