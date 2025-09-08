using Animancer;
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

        [SerializeField]
        private GameObject AhabSharkSprite;

        [SerializeField]
        private GameObject AhabNoSharkSprite;

        [SerializeField]
        private Animator sharkAnim;

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
        private LayerMask groundLayer;

        [SerializeField]
        private float lastStopped = Mathf.Infinity;

        [SerializeField]
        private float pickupCooldown = 1;

        //public bool throwing;

        [SerializeField]
        private float dashDamping = 8;

        [SerializeField]
        private float dashDuration = 1f;

        [SerializeField]
        private float timeSinceStartDash;

        [SerializeField]
        private float dashCooldown = 2;

        [SerializeField]
        private float biteCooldown = 2;

        [SerializeField]
        private BoxArea hitboxArea;

        [SerializeField]
        private HitboxEffect neutralAttackEffect;

        [SerializeField]
        private HitboxEffect dashAttackEffect;

        //[SerializeField]
        //private LayerMask groundLayer;

        [SerializeField]
        private LayerMask playerPickupLayer;

        [SerializeField]
        private UnityEvent onNeutralAttack;

        private float lastBite;

        //private bool dashOnCooldown = false;

        //private bool dashing;
        //private float initialLinearDamping;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            Physics2D.IgnoreCollision(physicsCollider, ahabPhysicsCollider);
            //initialLinearDamping = rb.linearDamping;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!thrown) //(!throwing && !thrown)
            {
                sprite.enabled = false;
            }
            else
            {
                sprite.enabled = true;
            }

            if (thrown)
            {
                /*if (dashing)
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
                }*/

                sprite.gameObject.SetActive(true);
                sprite.enabled = true;

                AlignSharkToVel();
            }

            timeSinceStartDash += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (onGround)
            {
                float velocityX = rb.linearVelocityX;
                velocityX = Mathf.MoveTowards(velocityX, 0, dashDamping * Time.deltaTime);
                rb.linearVelocity = new Vector2(velocityX, rb.linearVelocityY);
            }

            if (thrown && lastStopped == Mathf.Infinity && rb.linearVelocity == Vector2.zero)
            {
                lastStopped = Time.time + pickupCooldown;
            }
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
            else if (rb.linearVelocityX == 0 && playerPickupLayer.IsInLayerMask(other))
            {
                var special = other.gameObject.GetComponentInParent<AhabSpecialMove>();

                if (special == null)
                {
                    return;
                }

                if (special.sharkson != this)
                {
                    return;
                }

                if (lastStopped > Time.time)
                {
                    return;
                }

                PickUp();
            }
        }

        public void SpecialPressed()
        {
            if (thrown && Time.time >= lastBite)
            {
                SharkBiteGrab();
            }
            else
            {
                Debug.Log("Set as not active");
                sprite.gameObject.SetActive(false);
                //throwing = true;
            }
        }

        public void Throw(bool flipX, Vector2 position, Quaternion rotation, float throwForce)
        {
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
            //throwing = false;
            onGround = false;
            thrown = true;
            AhabNoSharkSprite.SetActive(true);
            AhabSharkSprite.SetActive(false);
            rb.simulated = true;
            lastBite = Time.time + biteCooldown;
        }

        public void SharkDash()
        {
            if (!thrown || timeSinceStartDash < dashCooldown)
            {
                return;
            }

            hitboxEmitter.EndHitboxGroup(neutralHitboxGroupId);
            lastStopped = Mathf.Infinity;
            timeSinceStartDash = 0;

            //rb.linearDamping = dashDamping;
            //rb.gravityScale = 0;
            onGround = false;
            characterRb.SetVelocityWithFlipX(ahabActionManager.CharacterActionInput.MoveInput * dashVelocity);
            //Invoke("SharkDashEnd", dashDuration);
        }

        public void SharkDashEnd()
        {
            //rb.gravityScale = 0.5f;
            //rb.linearDamping = initialLinearDamping;
            //hitboxEmitter.EndHitboxGroup(dashHitboxGroupId);
        }

        private void AlignSharkToVel()
        {
            if (rb.linearVelocity != Vector2.zero)
            {
                sprite.transform.localScale = new Vector3(Mathf.Sign(rb.linearVelocityX), 1, 1);
            }

            sprite.transform.right = rb.linearVelocity * Mathf.Sign(rb.linearVelocityX);
        }

        public void SharkBiteGrab()
        {
            hitboxEmitter.EndHitboxGroup(dashHitboxGroupId);
            HitboxContext context = hitboxEmitter.GetContext(dashHitboxGroupId);
            context.FlipX = characterRb.LinearVelocity.x < 0;
            rb.linearVelocity = Vector2.zero;

            hitboxEmitter.EmitHitbox(hitboxArea, dashAttackEffect, context, dashHitboxGroupId);
            lastBite = Time.time + biteCooldown;
            sharkAnim.Play("Sharkson_Bite");

            onNeutralAttack?.Invoke();
        }

        public void PickUp()
        {
            if (!thrown)
            {
                return;
            }

            Debug.Log("Picked up");

            //throwing = false;
            AhabSharkSprite.SetActive(true);
            AhabNoSharkSprite.SetActive(false);
            thrown = false;
            sprite.enabled = false;
            rb.simulated = false;
            lastStopped = Mathf.Infinity;
        }
    }
}