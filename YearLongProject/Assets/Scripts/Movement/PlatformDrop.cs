using System.Collections.Generic;
using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace Movement
{
    public class PlatformDrop : MonoBehaviour
    {
        [BoldHeader("Platform Dropdown")]
        [InfoBox("Dropping down through platforms logic")]
        [Header("Depends")]

        [SerializeField]
        private Collider2D characterPhysicsCollider;

        [Header("Config")]

        [SerializeField]
        private LayerMask platformDropIgnoredLayer;

        [SerializeField]
        private float dropBufferTime;

        [SerializeField]
        private float dropInputAngleFromVertical;

        private readonly List<Collider2D> ignoredColliders = new();

        [Header("Debug")]

        [ShowNonSerializedField]
        private bool droppingThroughPlatform;

        [ShowNonSerializedField]
        private float dropLockInTimer;

        private void Update()
        {
            if (dropLockInTimer > 0)
            {
                dropLockInTimer -= Time.deltaTime;
                if (dropLockInTimer <= 0)
                {
                    foreach (Collider2D ignoredCollider in ignoredColliders)
                    {
                        Physics2D.IgnoreCollision(characterPhysicsCollider, ignoredCollider, false);
                    }

                    ignoredColliders.Clear();
                }
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            int otherLayer = other.gameObject.layer;
            if (platformDropIgnoredLayer == (platformDropIgnoredLayer | (1 << otherLayer)))
            {
                dropLockInTimer = dropBufferTime;
                if (ignoredColliders.Contains(other.collider))
                {
                    return;
                }

                if (droppingThroughPlatform)
                {
                    ignoredColliders.Add(other.collider);
                    Physics2D.IgnoreCollision(characterPhysicsCollider, other.collider, true);
                }
            }
        }

        public void HandleMoveInput(Vector2 input)
        {
            float angleFromVertical = Vector2.Angle(Vector2.down, input);
            if (angleFromVertical < dropInputAngleFromVertical && input.y < 0f)
            {
                droppingThroughPlatform = true;
            }
            else
            {
                droppingThroughPlatform = false;

                if (dropLockInTimer <= 0f)
                {
                    foreach (Collider2D ignoredCollider in ignoredColliders)
                    {
                        Physics2D.IgnoreCollision(characterPhysicsCollider, ignoredCollider, false);
                    }

                    ignoredColliders.Clear();
                }
            }
        }
    }
}