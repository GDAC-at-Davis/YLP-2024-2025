using UnityEngine;

namespace State_Machine_Scripts
{
    public class SimpleMovementController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5;

        [SerializeField]
        private Rigidbody2D characterRigidbody;

        [SerializeField]
        private LayerMask groundLayer;

        [SerializeField]
        private float groundCheckDistance;
        public float stunTime;

        public Vector2 Knockback { get; set; }

        private Vector2 Position => characterRigidbody ? characterRigidbody.position : Vector2.zero;

        private bool inJump;
        private bool isGrounded;
        private float playerMove;
        private float jumpVelocity;

        private void FixedUpdate()
        {
            isGrounded = Physics2D.Raycast(Position, -Vector2.up, groundCheckDistance, groundLayer);

            float playerIntendedMove = playerMove * speed;
            float newVelocity = Mathf.Lerp(characterRigidbody.linearVelocityX, playerIntendedMove, 0.2f);
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

        public void SetCharacterMove(float playerMove)
        {
            this.playerMove = playerMove;
        }

        public void SetJumpVelocity(float jumpVelocity)
        {
            this.jumpVelocity = jumpVelocity;
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
        }

        public void SetVelocity(Vector2 velocity)
        {
        }

        public void SetHorizontalVelocity(float velocity)
        {
            Vector3 curVel = characterRigidbody.linearVelocity;
            characterRigidbody.linearVelocity = new Vector2(velocity, curVel.y);
        }

        public void SetVerticalVelocity(float velocity)
        {
            Vector3 curVel = characterRigidbody.linearVelocity;
            characterRigidbody.linearVelocity = new Vector2(curVel.x, velocity);
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