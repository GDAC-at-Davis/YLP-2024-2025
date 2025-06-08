using GameEntities;
using Hitbox.DataStructures;
using Movement;
using UnityEngine;
using Hitbox.Emitters;
using Hitbox.HitboxAreas;
using Hitbox.System;

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
        private Collider2D col;

        [Header("Hitbox")]
        [SerializeField]
        private BasicHitboxEmitter hitboxEmitter;
        [SerializeField]
        private string hitboxGroupID;
        [SerializeField]
        BoxArea hitboxArea;
        [SerializeField]
        HitboxEffect hitboxEffect;


        public void Initialize(WizardOrbManager manager)
        {
            this.manager = manager;
            gameObject.SetActive(false);

            rb = GetComponent<CharacterRigidbody2D>();
            col = GetComponent<Collider2D>();
            anim = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            DoCollisions();
            Rebounds();

            rb.LinearVelocity = Vector3.MoveTowards(rb.LinearVelocity, Vector3.zero, slowdownRate * Time.deltaTime);

            if (rb.LinearVelocity == Vector2.zero)
            {
                hitboxEmitter.EndHitboxGroup(hitboxGroupID);
                return;
            }
            else
            {
                HitboxContext context = hitboxEmitter.GetContext(hitboxGroupID);
                context.FlipX = rb.LinearVelocity.x < 0;
                hitboxEmitter.EmitHitbox(hitboxArea, hitboxEffect, context, hitboxGroupID);
            }
        }

        public override void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact)
        {
            if (hitboxInstance.Context.Source != manager.Wizard)
            {
                return;
            }

            int dir = hitboxInstance.Context.FlipX ? -1 : 1;

            if (hitboxInstance.HitboxEffect.Damage < 5 && hitboxInstance.HitboxEffect.Damage > 2) // hit by light
            {
                Vector2 kb = lightTravelSpeed;
                kb.x *= dir;
                rb.LinearVelocity = kb;
            }
            else if (hitboxInstance.HitboxEffect.Damage > 5) // hit by heavy
            {
                Vector2 kb = heavyTravelSpeed;
                kb.x *= dir;
                rb.LinearVelocity = kb;
            }
        }

        private void DoCollisions() // I couldn't figure out how to get terrain collisions to work without letting players pass thru sorry
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

        public void Impact()
        {
            rb.LinearVelocity = Vector2.zero;
            anim.Play("OrbImpact");
        }
        public void DestroyBall()
        {
            manager.ResetOrb(this);
            Reset();
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            anim.Play("OrbAnim");
            rb.LinearVelocity = Vector2.zero;
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
