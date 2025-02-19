using Base;
using UnityEngine;

namespace Movement
{
    /// <summary>
    ///     Managed wrapper around Rigidbody2D
    /// </summary>
    [DefaultExecutionOrder(1000)]
    public class CharacterRigidbody2D : DescriptionMono
    {
        [SerializeField]
        private Rigidbody2D rb2D;

        [SerializeField]
        private Vector2 gravityAcceleration;

        public Vector2 LinearVelocity
        {
            get => rb2D.linearVelocity;
            set
            {
                if (isFrozen)
                {
                    cachedVelocity = value;
                }
                else
                {
                    rb2D.linearVelocity = value;
                }
            }
        }

        private Vector2 movePositionAccumulator;

        private int xFlipTransform = 1;
        private bool isFrozen;
        private Vector2 cachedVelocity;

        private void FixedUpdate()
        {
            rb2D.linearVelocity += gravityAcceleration * Time.fixedDeltaTime;
            if (movePositionAccumulator != Vector2.zero)
            {
                // We need to use an accumulator since multiple MovePositions in a single physics update overwrite each other
                // Instead of adding
                Vector2 vel = rb2D.linearVelocity;
                rb2D.MovePosition(rb2D.position + movePositionAccumulator + vel * Time.fixedDeltaTime);
                movePositionAccumulator = Vector2.zero;
            }
        }

        public void SetFlipX(bool flipX)
        {
            xFlipTransform = flipX ? -1 : 1;
        }

        /// <summary>
        ///     Set the frozen state of the rigidbody. Currently should only be used for hit stop
        /// </summary>
        /// <param name="frozen"></param>
        public void SetFrozen(bool frozen)
        {
            if (isFrozen == frozen)
            {
                return;
            }

            if (frozen)
            {
                cachedVelocity = LinearVelocity;
            }

            isFrozen = frozen;
            rb2D.constraints = frozen ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;

            if (!frozen)
            {
                LinearVelocity = cachedVelocity;
            }
        }

        /// <summary>
        ///     Move the rigidbody by some amount, flipping the X based on the current flip state
        /// </summary>
        /// <param name="moveAmount"></param>
        public void MoveRelativeWithFlipX(Vector2 moveAmount)
        {
            moveAmount.x *= xFlipTransform;
            movePositionAccumulator += moveAmount;
        }
    }
}