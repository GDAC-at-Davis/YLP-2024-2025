using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace Movement
{
    public class FastFall : MonoBehaviour
    {
        [BoldHeader("Fast-Fall Script")]
        [InfoBox("Handles fast-fall logic when pressing down. ")]
        [Header("Dependencies")]

        [SerializeField]
        private CharacterRigidbody2D characterRigidbody2D;

        [Header("Configuration")]

        [SerializeField]
        private float fastFallddedAcceleration;

        [SerializeField]
        private float fastFallTerminalVelocity;

        private Vector2 originalGravity;

        private bool inFastFall;

        private void FixedUpdate()
        {
            if (inFastFall)
            {
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
            if (input.y < 0)
            {
                inFastFall = true;
            }
            else
            {
                inFastFall = false;
            }
        }
    }
}