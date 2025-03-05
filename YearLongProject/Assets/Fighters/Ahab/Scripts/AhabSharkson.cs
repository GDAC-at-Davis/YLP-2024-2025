using State_Machine_Scripts;
using UnityEngine;

namespace Fighters.Ahab.Scripts
{
    public class AhabSharkson : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField]
        private CharacterActionManager ahabActionManager;

        [SerializeField]
        private float dashVelocity;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            //this.gameObject.SetActive(false);
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

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.layer == 6)
            {
                Debug.Log("hit ground");
            }
            else if (other.gameObject.layer == 3)
            {
                Debug.Log("Pickup Sharkson");
                var special = other.gameObject.GetComponentInChildren<AhabSpecialMove>();
                if (special != null)
                {
                    if (special.sharkson == this)
                    {
                        PickUp(special.throwTransform.gameObject);
                    }
                }
            }
        }

        public void Throw(float throwForce)
        {
            gameObject.SetActive(true);
            gameObject.transform.parent = null;
            rb.simulated = true;
            rb.AddForce(transform.right * throwForce, ForceMode2D.Impulse);
        }

        public void SharkDash()
        {
            rb.simulated = true;
            rb.AddForce(transform.right * dashVelocity, ForceMode2D.Impulse);
        }

        public void PickUp(GameObject parent)
        {
            gameObject.SetActive(false);
            rb.simulated = false;
            gameObject.transform.parent = parent.transform;
        }
    }
}