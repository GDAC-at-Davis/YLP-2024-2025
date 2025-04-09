using Animancer;
using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Movement
{
    public class FastFall : MonoBehaviour
    {
        [BoldHeader("Fast-Fall Script")]
        [InfoBox("Handles fast-fall logic when pressing down. \n Modify the Fast Fall stats here.")]
        [Header("Dependencies")]

        [SerializeField]
        private CharacterRigidbody2D characterRigidbody2D;

        [FormerlySerializedAs("fastFallddedAcceleration")]
        [Header("Configuration")]

        [SerializeField]
        private float fastFallAddedAcceleration;

        [SerializeField]
        private float fastFallTerminalVelocity;

        [InfoBox("Angle range from completely vertical down input where fastfall is triggered")]
        [SerializeField]
        private float fastFallInputAngle;

        [InfoBox("Fastfall is only triggered when the player's Y velocity is already below this value")]
        [SerializeField]
        private float fastFallThresholdVelocity;

        [Header("Events")]

        [SerializeField]
        private UnityEvent onFastFallStart;

        [SerializeField]
        private UnityEvent onFastFallEnd;

        private bool CanEnterFastFall => fastFallEnabled && isFalling;

        private Vector2 originalGravity;

        private bool fastFallInputDown;
        private bool fastFallInputHeld;

        [ShowNonSerializedField]
        private bool fastFallEnabled;

        private bool isFalling;

        private bool isFastFalling;

        private void FixedUpdate()
        {
            isFalling = characterRigidbody2D.LinearVelocity.y < fastFallThresholdVelocity;
            bool shouldFastFall = fastFallInputDown && CanEnterFastFall;

            if (!isFastFalling && shouldFastFall)
            {
                onFastFallStart?.Invoke();
            }
            else if (isFastFalling && !shouldFastFall)
            {
                onFastFallEnd?.Invoke();
            }

            isFastFalling = shouldFastFall;

            if (shouldFastFall)
            {
                // Don't set the velocity directly, as it will override the gravity
                // Fastfall is just an additional acceleration
                float cVelY = characterRigidbody2D.LinearVelocity.y;
                if (cVelY > -fastFallTerminalVelocity)
                {
                    characterRigidbody2D.LinearVelocity +=
                        Vector2.down * fastFallAddedAcceleration * Time.fixedDeltaTime;
                }
            }
        }

        public void HandleMoveInput(Vector2 input)
        {
            if (input.magnitude < 0.01f)
            {
                fastFallInputDown = false;
                fastFallInputHeld = false;
                return;
            }

            float angle = Vector2.Angle(Vector2.down, input);
            if (angle < fastFallInputAngle)
            {
                // To enter fastfall, the input must be pressed when fastfall is allowed
                // i.e you cannot always hold down to enter fastfall the moment it becomes allowed
                if (fastFallInputHeld == false && CanEnterFastFall)
                {
                    fastFallInputDown = true;
                }

                fastFallInputHeld = true;
            }
            else
            {
                fastFallInputDown = false;
                fastFallInputHeld = false;
            }

            if (!CanEnterFastFall)
            {
                fastFallInputDown = false;
            }
        }

        public void SetFastFallEnabled(bool enabled)
        {
            fastFallEnabled = enabled;
        }
    }
}