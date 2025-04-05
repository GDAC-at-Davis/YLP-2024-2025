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

        private Vector2 originalGravity;

        private bool fastFallInput;

        [ShowNonSerializedField]
        private bool fastFallEnabled;

        private bool isFastFalling;

        private void FixedUpdate()
        {
            bool isFalling = characterRigidbody2D.LinearVelocity.y < fastFallThresholdVelocity;
            bool shouldFastFall = fastFallInput && fastFallEnabled && isFalling;

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
            float angle = Vector2.Angle(Vector2.down, input);
            if (angle < fastFallInputAngle)
            {
                fastFallInput = true;
            }
            else
            {
                fastFallInput = false;
            }
        }

        public void SetFastFallEnabled(bool enabled)
        {
            fastFallEnabled = enabled;
        }
    }
}