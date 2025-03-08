using Movement;
using UnityEngine;

namespace State_Machine_Scripts
{
    public class SimpleMovementController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5;

        [SerializeField]
        private float acceleration;

        [SerializeField]
        private CharacterRigidbody2D characterRigidbody;

        [SerializeField]
        private LayerMask groundLayer;

        [SerializeField]
        private float groundCheckDistance;

        public float stunTime;

        private Vector2 Position => characterRigidbody ? characterRigidbody.Position : Vector2.zero;

        private bool inJump;
        private bool isGrounded;
        private float horizontalInput;
        private float jumpVelocity;

        private void FixedUpdate()
        {
            isGrounded = Physics2D.Raycast(Position, -Vector2.up, groundCheckDistance, groundLayer);

            float playerIntendedMove = horizontalInput * speed;
            float newVelocity = Mathf.Lerp(characterRigidbody.LinearVelocity.x, playerIntendedMove,
                acceleration * Time.fixedDeltaTime);
            SetHorizontalVelocity(newVelocity);

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
            if (desiredMove == 0)
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