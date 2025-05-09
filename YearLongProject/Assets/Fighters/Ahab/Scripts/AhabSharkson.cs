using EditorUtils.BoldHeader;
using NaughtyAttributes;
using State_Machine_Scripts;
using UnityEngine;
using Movement;
using Hitbox;
using Hitbox.HitboxAreas;
using Hitbox.Emitters;
using Hitbox.DataStructures;

namespace Fighters.Ahab.Scripts
{
    public class AhabSharkson : MonoBehaviour
    {
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

        [Header("Colliders")]

        [InfoBox("Colliders that need to ignore each other, so SHARKSON doesn't hit Ahab")]
        [SerializeField]
        private Collider2D physicsCollider;

        [SerializeField]
        private Collider2D ahabPhysicsCollider;

        [Header("Config")]

        [SerializeField]
        private float dashVelocity;

        public bool thrown = false;
        [SerializeField]
        private bool onGround = false;

        private bool dashOnCooldown = false;

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
        private BasicHitboxEmitter hitboxEmitter;

        [SerializeField]
        private BoxArea hitboxArea;

        [SerializeField]
        private HitboxEffect neutralAttackEffect;

        [SerializeField]
        private HitboxEffect dashAttackEffect;

        [SerializeField]
        private CharacterActionManager actionManager;

        private bool dashing;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            Physics2D.IgnoreCollision(physicsCollider, ahabPhysicsCollider);
        }

        // Update is called once per frame
        private void Update()
        {
            if(!throwing && !thrown)
            {
                sprite.enabled = false;
            }
            else
            {
                sprite.enabled = true;
            }

            if(thrown)
            {
                if(dashing)
                {
                    hitboxEmitter.EmitHitbox(hitboxArea, dashAttackEffect, "sharkDash");
                }
                else
                {
                    hitboxEmitter.EmitHitbox(hitboxArea, neutralAttackEffect, "sharkImpact");
                }

                sprite.gameObject.SetActive(true);
                sprite.enabled = true;

                if (rb.linearVelocityX >= 0)
                {
                    transform.right = rb.linearVelocity;
                    sprite.flipX = false;
                }
                else
                {
                    transform.right = -rb.linearVelocity;
                    sprite.flipX = true;
                }
            }

            timeSinceStartDash += Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!thrown) return;

            if (other.gameObject.layer == 6)
            {
                Debug.Log("hit ground");
                onGround = true;
            }
            else if (other.gameObject.layer == 3)
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
            if(thrown)
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

            sprite.flipX = flipX;

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
            if(!thrown || timeSinceStartDash < dashCooldown)
            {
                return;
            }

            hitboxEmitter.EndHitboxGroup("sharkImpact");

            timeSinceStartDash = 0;

            rb.linearDamping = dashDamping;
            //rb.gravityScale = 0;
            characterRb.SetVelocityWithFlipX(ahabActionManager.CharacterActionInput.MoveInput * dashVelocity);
            Invoke("SharkDashEnd", dashDuration);
        }

        public void SharkDashEnd()
        {
            //rb.gravityScale = 0.5f;
            rb.linearDamping = 0.5f;
            hitboxEmitter.EndHitboxGroup("sharkDash");
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