using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace Movement
{
    public class FastFall : MonoBehaviour
    {
        [BoldHeader("Fast-Fall Script")]
        [InfoBox("Handles fast-fall logic when pressing down. \n Modify the Fast Fall stats here.")]
        [Header("Dependencies")]

        [SerializeField]
        private CharacterRigidbody2D characterRigidbody2D;

        [Header("Configuration")]

        [SerializeField]
        private float fastFallddedAcceleration;

        [SerializeField]
        private float fastFallTerminalVelocity;

        [InfoBox("Angle range from completely vertical down input where fastfall is triggered")]
        [SerializeField]
        private float fastFallInputAngle;

        [InfoBox("Fastfall is only triggered when the player's Y velocity is already below this value")]
        [SerializeField]
        private float fastFallThresholdVelocity;

        private Vector2 originalGravity;

        private bool inFastFall;

        [ShowNonSerializedField]
        private bool fastFallEnabled;

        private void FixedUpdate()
        {
            bool isFalling = characterRigidbody2D.LinearVelocity.y < fastFallThresholdVelocity;
            if (inFastFall && fastFallEnabled && isFalling)
            {
                // Don't set the velocity directly, as it will override the gravity
                // Fastfall is just an additional acceleration
                float cVelY = characterRigidbody2D.LinearVelocity.y;
                if (cVelY > -fastFallTerminalVelocity)
                {
                    characterRigidbody2D.LinearVelocity +=
                        Vector2.down * fastFallddedAcceleration * Time.fixedDeltaTime;
                }
            }
        }

        public void HandleMoveInput(Vector2 input)
        {
            float angle = Vector2.SignedAngle(Vector2.down, input);
            if (angle < fastFallInputAngle)
            {
                inFastFall = true;
            }
            else
            {
                inFastFall = false;
            }
        }

        public void SetFastFallEnabled(bool enabled)
        {
            fastFallEnabled = enabled;
        }
    }
}