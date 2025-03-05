using EditorUtils.BoldHeader;
using NaughtyAttributes;
using State_Machine_Scripts;
using UnityEngine;

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

        [Header("Colliders")]

        [InfoBox("Colliders that need to ignore each other, so SHARKSON doesn't hit Ahab")]
        [SerializeField]
        private Collider2D physicsCollider;

        [SerializeField]
        private Collider2D ahabPhysicsCollider;

        [Header("Config")]

        [SerializeField]
        private float dashVelocity;

        private bool thrown;
        private bool onGround;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            Physics2D.IgnoreCollision(physicsCollider, ahabPhysicsCollider);
        }

        // Update is called once per frame
        private void Update()
        {
            if (rb.simulated)
            {
                transform.right = rb.linearVelocity;
                if (rb.linearVelocityX >= 0)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 6)
            {
                Debug.Log("hit ground");
                onGround = true;
            }
            else if (other.gameObject.layer == 3)
            {
                var special = other.gameObject.GetComponentInParent<AhabSharkson>();

                if (special == null)
                {
                    return;
                }
                /*if (special.sharkson == this)
                {
                    PickUp();
                }*/
            }
        }

        public void Throw(Vector2 position, Quaternion rotation, float throwForce)
        {
            if (thrown)
            {
                return;
            }

            transform.SetPositionAndRotation(position, rotation);

            thrown = true;
            onGround = false;

            gameObject.SetActive(true);
            rb.simulated = true;
            rb.AddForce(transform.right * throwForce, ForceMode2D.Impulse);
        }


        // Comment
        public void SharkDash()
        {
            rb.simulated = true;
            rb.AddForce(transform.right * dashVelocity, ForceMode2D.Impulse);
        }

        public void PickUp()
        {
            if (!onGround || !thrown)
            {
                return;
            }

            thrown = false;
            gameObject.SetActive(false);
            rb.simulated = false;
        }
    }
}