using EditorUtils.BoldHeader;
using Hitbox.DataStructures;
using Hitbox.Emitters;
using Hitbox.HitboxAreas;
using Movement;
using NaughtyAttributes;
using State_Machine_Scripts;
using UnityEngine;
using Utils;

namespace Fighters.Ahab.Scripts
{
    public class AhabSharkson : MonoBehaviour
    {
        private const string dashHitboxGroupId = "sharkDash";
        private const string neutralHitboxGroupId = "sharkImpact";

        [BoldHeader("SHARKSON Script")]
        [Header("Dependencies")]

        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField]
        private CharacterActionManager ahabActionManager;

        [SerializeField]
        private CharacterRigidbody2D characterRb;

        [SerializeField]
        private CharacterActionManager actionManager;

        [SerializeField]
        private BasicHitboxEmitter hitboxEmitter;

        [Header("Colliders")]

        [InfoBox("Colliders that need to ignore each other, so SHARKSON doesn't hit Ahab")]
        [SerializeField]
        private Collider2D physicsCollider;

        [SerializeField]
        private Collider2D ahabPhysicsCollider;

        [Header("Config")]

        [SerializeField]
        private float dashVelocity;

        public bool thrown;

        [SerializeField]
        private bool onGround;

        [SerializeField]
        private bool throwing;

        [SerializeField]
        private float dashDamping = 8;

        [SerializeField]
        private float dashDuration = 1f;

        [SerializeField]
        private float timeSinceStartDash;

        [SerializeField]
        private float dashCooldown = 2;

        [SerializeField]
        private BoxArea hitboxArea;

        [SerializeField]
        private HitboxEffect neutralAttackEffect;

        [SerializeField]
        private HitboxEffect dashAttackEffect;

        [SerializeField]
        private LayerMask groundLayer;

        [SerializeField]
        private LayerMask playerPickupLayer;

        private bool dashOnCooldown = false;

        private bool dashing;
        private float initialLinearDamping;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            Physics2D.IgnoreCollision(physicsCollider, ahabPhysicsCollider);
            initialLinearDamping = rb.linearDamping;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!throwing && !thrown)
            {
                sprite.enabled = false;
            }
            else
            {
                sprite.enabled = true;
            }

            if (thrown)
            {
                if (dashing)
                {
                    HitboxContext context = hitboxEmitter.GetContext(dashHitboxGroupId);
                    context.FlipX = characterRb.LinearVelocity.x < 0;
                    hitboxEmitter.EmitHitbox(hitboxArea, dashAttackEffect, context, dashHitboxGroupId);
                }
                else
                {
                    HitboxContext context = hitboxEmitter.GetContext(neutralHitboxGroupId);
                    context.FlipX = characterRb.LinearVelocity.x < 0;
                    hitboxEmitter.EmitHitbox(hitboxArea, neutralAttackEffect, context, neutralHitboxGroupId);
                }

                sprite.gameObject.SetActive(true);
                sprite.enabled = true;

                if (rb.linearVelocityX >= 0)
                {
                    transform.right = rb.linearVelocity;
                    AlignSharkToVel();
                }
                else
                {
                    transform.right = -rb.linearVelocity;
                    AlignSharkToVel();
                }
            }

            timeSinceStartDash += Time.deltaTime;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!thrown)
            {
                return;
            }

            if (groundLayer.IsInLayerMask(other) && characterRb.LinearVelocity.y <= 0)
            {
                onGround = true;
            }
            else if (playerPickupLayer.IsInLayerMask(other))
            {
                var special = other.gameObject.GetComponentInParent<AhabSpecialMove>();

                if (special == null)
                {
                    return;
                }

                if (special.sharkson == this)
                {
                    PickUp();
                }
            }
        }

        public void SpecialPressed()
        {
            if (thrown)
            {
                SharkBiteGrab();
            }
            else
            {
                Debug.Log("Set as not active");
                sprite.gameObject.SetActive(false);
                throwing = true;
            }
        }

        public void Throw(bool flipX, Vector2 position, Quaternion rotation, float throwForce)
        {
            throwing = false;
            thrown = true;
            onGround = false;

            rb.simulated = true;

            AlignSharkToVel();

            if (!flipX)
            {
                transform.SetPositionAndRotation(position, rotation);
            }
            else
            {
                transform.SetPositionAndRotation(position, Quaternion.Euler(-rotation.eulerAngles));
            }

            characterRb.SetVelocityWithFlipX(transform.right * throwForce);
        }

        public void SharkDash()
        {
            if (!thrown || timeSinceStartDash < dashCooldown)
            {
                return;
            }

            hitboxEmitter.EndHitboxGroup(neutralHitboxGroupId);

            timeSinceStartDash = 0;

            rb.linearDamping = dashDamping;
            //rb.gravityScale = 0;
            characterRb.SetVelocityWithFlipX(ahabActionManager.CharacterActionInput.MoveInput * dashVelocity);
            Invoke("SharkDashEnd", dashDuration);
        }

        public void SharkDashEnd()
        {
            //rb.gravityScale = 0.5f;
            rb.linearDamping = initialLinearDamping;
            hitboxEmitter.EndHitboxGroup(dashHitboxGroupId);
        }

        private void AlignSharkToVel()
        {
            if (rb.linearVelocity.x == 0)
            {
                return;
            }

            bool flipX = rb.linearVelocity.x < 0;
            sprite.transform.localScale = new Vector3(flipX ? -1 : 1, 1, 1);
        }

        public void SharkBiteGrab()
        {
            //rb.
        }

        public void PickUp()
        {
            if (!onGround || !thrown)
            {
                return;
            }

            Debug.Log("Picked up");

            thrown = false;
            sprite.enabled = false;
            rb.simulated = false;
        }
    }
}