using GameEntities;
using Hitbox.DataStructures;
using Movement;
using UnityEngine;
using Hitbox.Emitters;
using Hitbox.HitboxAreas;
using Hitbox.System;
using Hitbox;
using State_Machine_Scripts;
using State_Machine_Scripts.States;

namespace Fighters.Wizard.Scripts
{
    public class WizardOrbBehavior : Entity
    {
        private WizardOrbManager manager;
        private Animator anim;

        [Header("Orb Properties")]
        [SerializeField]
        private float slowdownRate = 1.5f;
        [SerializeField]
        private float rayDistance = 0.05f;
        [SerializeField]
        private Vector2 heavyTravelSpeed;
        [SerializeField]
        private Vector2 lightTravelSpeed;

        [Header("Collisions")]
        [SerializeField]
        private LayerMask physicsCollisionLayers;
        Collisions collisions;
        private CharacterRigidbody2D rb;
        private BoxCollider2D col;
        public BoxCollider2D Collider => col;

        [Header("Hitbox")]
        [SerializeField]
        private BasicHitboxEmitter hitboxEmitter;
        [SerializeField]
        private string hitboxGroupID;
        [SerializeField]
        BoxArea hitboxArea;
        BoxArea knockbackHitboxArea;
        [SerializeField]
        Vector2 knockbackHitboxSize;
        [SerializeField]
        HitboxEffect hitboxEffect;

        [Header("Effects")]
        [SerializeField]
        private ParticleSystem particleSystem;


        public void Initialize(WizardOrbManager manager)
        {
            this.manager = manager;
            gameObject.SetActive(false);

            rb = GetComponent<CharacterRigidbody2D>();
            col = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
            knockbackHitboxArea = new BoxArea(Vector2.zero, 0, knockbackHitboxSize);
        }

        private void FixedUpdate()
        {
            DoCollisions();
            Rebounds();

            rb.LinearVelocity = Vector3.MoveTowards(rb.LinearVelocity, Vector3.zero, slowdownRate * Time.deltaTime);

            if (rb.LinearVelocity == Vector2.zero)
            {
                CheckForPlayerCollisions(Physics2D.BoxCastAll(transform.position, knockbackHitboxSize, 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player")));
                return;
            }
            else
            {
                HitboxContext context = hitboxEmitter.GetContext(hitboxGroupID);
                context.FlipX = rb.LinearVelocity.x < 0;
                hitboxEmitter.EmitHitbox(hitboxArea, hitboxEffect, context, hitboxGroupID);
            }
        }

        // move orb when hit by wizard
        public override void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact)
        {
            if (hitboxInstance.Context.Source != manager.Wizard)
            {
                return;
            }

            int dir = hitboxInstance.Context.FlipX ? -1 : 1;
            Vector2 kb = Vector2.zero;

            switch (hitboxInstance.Context.HitboxID)
            {
                case "LightPrimary3":
                    kb = lightTravelSpeed;
                    kb.x *= dir;
                    break;
                case "HeavyPrimary":
                    kb = heavyTravelSpeed;
                    kb.x *= dir;
                    break;
                case "WizardSpecial2":
                    kb = lightTravelSpeed;
                    kb.x *= dir;
                    kb.y *= -1;
                    break;
                default:
                    break;
            }

            LaunchBall(kb);
        }

        private void LaunchBall(Vector2 velocity)
        {
            // Don't launch ball if already detonating
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("OrbImpact"))
            {
                return;
            }
            anim.Play("OrbInteract");
            rb.LinearVelocity = velocity;
        }

        private void DoCollisions() // I couldn't figure out the weird collision interacions so I'm just doing this sorry
        {
            collisions.right = collisions.left = collisions.above = collisions.below = false;

            collisions.right = Physics2D.Raycast(transform.position + (Vector3.right * col.bounds.size.x / 2), Vector3.right, rayDistance, physicsCollisionLayers);
            collisions.left = Physics2D.Raycast(transform.position + (Vector3.left * col.bounds.size.x / 2), Vector3.left, rayDistance, physicsCollisionLayers);
            collisions.above = Physics2D.Raycast(transform.position + (Vector3.up * col.bounds.size.y / 2), Vector3.up, rayDistance, physicsCollisionLayers);
            collisions.below = Physics2D.Raycast(transform.position + (Vector3.down * col.bounds.size.y / 2), Vector3.down, rayDistance, physicsCollisionLayers);
            Debug.DrawRay(transform.position + (Vector3.right * col.bounds.size.x / 2), Vector3.right * rayDistance);
            Debug.DrawRay(transform.position + (Vector3.left * col.bounds.size.x / 2), Vector3.left * rayDistance);
            Debug.DrawRay(transform.position + (Vector3.up * col.bounds.size.y / 2), Vector3.up * rayDistance);
            Debug.DrawRay(transform.position + (Vector3.down * col.bounds.size.y / 2), Vector3.down * rayDistance);
        }

        private void Rebounds()
        {
            if ((collisions.above && rb.LinearVelocity.y > 0) || (collisions.below && rb.LinearVelocity.y < 0))
            {
                Vector2 kb = rb.LinearVelocity;
                kb.y *= -1;
                rb.LinearVelocity = kb;
            }

            if ((collisions.left && rb.LinearVelocity.x < 0) || (collisions.right && rb.LinearVelocity.x > 0))
            {
                Vector2 kb = rb.LinearVelocity;
                kb.x *= -1;
                rb.LinearVelocity = kb;
            }
        }

        // Helper functions called by animator/timeline events
        public void Impact()
        {
            rb.LinearVelocity = Vector2.zero;
            anim.Play("OrbImpact");
            particleSystem.Play();
            particleSystem.transform.parent = null;
        }
        public void Detonate(Vector3 target)
        {
            HitboxContext context = hitboxEmitter.GetContext(hitboxGroupID);
            context.FlipX = target.x < 0;
            hitboxEmitter.EmitHitbox(knockbackHitboxArea, hitboxEffect, context, hitboxGroupID);
            Impact();
        }
        public void DestroyBall()
        {
            manager.ResetOrb(this);
            Reset();
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            anim.Play("OrbInteract");
            particleSystem.Stop();
            if (!gameObject.activeSelf)
            {
                particleSystem.transform.parent = transform;
                particleSystem.transform.localPosition = Vector3.zero;
            }
            hitboxEmitter.EndHitboxGroup(hitboxGroupID);
            rb.LinearVelocity = Vector2.zero;
        }

        private void CheckForPlayerCollisions(RaycastHit2D[] hits)
        {
            foreach (RaycastHit2D hit in hits)
            {
                CharacterEntity manager = (CharacterEntity)(hit.collider.GetComponent<EntityHurtbox>().AttachedEntity);
                if (manager == this.manager.Wizard) return;

                //if (manager.CurrentState.stateNameSO.StateType == State_Machine_Scripts.StateNameSO.StateTypes.HITSTUN)
                if (manager.ActionManager.CurrentState.GetType() != typeof(HitstunState)) return;
                Detonate(manager.MovementController.CharacterRigidbody.LinearVelocity);
            }
        }

        struct Collisions
        {
            public bool above;
            public bool below;
            public bool left;
            public bool right;
        }
    }
}
