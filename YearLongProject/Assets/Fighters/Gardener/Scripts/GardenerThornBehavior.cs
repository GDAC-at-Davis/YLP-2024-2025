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

namespace Fighters.Gardener.Scripts
{
    public class GardenerThornBehavior : MonoBehaviour
    {
        private GardenerThornManager manager;
        private Animator anim;

        [Header("Thorn Properties")]
        [SerializeField]
        private Vector2 projectileSpeed;
        [SerializeField]
        private float gravity;
        [SerializeField]
        private float rayDistance = 0.05f;
        [SerializeField]
        private float projectileLifetime = 3;
        private float destroyProjectile;
        private bool attached;
        private int direction = 1;
        public bool Attached => attached;
        public int Direction { get => direction; set => direction = value; }

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
        [SerializeField]
        HitboxEffect hitboxEffect;

        [Header("Effects")]
        [SerializeField]
        private ParticleSystem particleSystem;

        Vector2 velocity;

        public void Initialize(GardenerThornManager manager)
        {
            this.manager = manager;
            gameObject.SetActive(false);

            rb = GetComponent<CharacterRigidbody2D>();
            col = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (attached) return;
            if (Time.time >= destroyProjectile)
            {
                gameObject.SetActive(false);
                return;
            }
            velocity = rb.LinearVelocity;
            velocity.y -= gravity;
            rb.LinearVelocity = velocity;

            DoCollisions();
            CheckForEnvironmentCollisions();
            CheckForPlayerCollisions(Physics2D.BoxCastAll(transform.position, col.size * 1.5f, 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player")));
        }

        private void OnEnable()
        {
            rb.LinearVelocity = new Vector2(projectileSpeed.x * direction, projectileSpeed.y);
            destroyProjectile = Time.time + projectileLifetime;
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

        private void CheckForEnvironmentCollisions()
        {
            if ((collisions.above && rb.LinearVelocity.y > 0) || (collisions.below && rb.LinearVelocity.y < 0))
            {
                Vector2 kb = rb.LinearVelocity;
                kb.y = kb.y * -1 / 1.2f;
                rb.LinearVelocity = kb;
            }

            if (((collisions.left && rb.LinearVelocity.x < 0) || (collisions.right && rb.LinearVelocity.x > 0)) && !collisions.below)
            {
                Vector2 kb = rb.LinearVelocity;
                kb.x *= -1;
                rb.LinearVelocity = kb;
            }
        }

        private void CheckForPlayerCollisions(RaycastHit2D[] hits)
        {
            foreach (RaycastHit2D hit in hits)
            {
                CharacterEntity manager = (CharacterEntity)(hit.collider.GetComponent<EntityHurtbox>().AttachedEntity);
                if (manager == this.manager.Gardener) return;

                Debug.Log("touch");
                Attach(manager);
            }
        }
        private void Attach(CharacterEntity entity)
        {
            transform.parent = entity.MovementController.CharacterRigidbody.transform;
            rb.LinearVelocity = Vector2.zero;
            attached = true;
        }

        public void Warp()
        {
            hitboxEmitter.EmitHitbox(hitboxArea, hitboxEffect, hitboxGroupID);
            hitboxEmitter.EndHitboxGroup(hitboxGroupID);
            transform.parent = null;
            gameObject.SetActive(false);
            attached = false;
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
