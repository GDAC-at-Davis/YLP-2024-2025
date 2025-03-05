using UnityEngine;
using State_Machine_Scripts;

namespace State_Machine_Scripts.States {
    public class AhabSharkson : MonoBehaviour
    {
        [SerializeField]
        Rigidbody2D rb;

        [SerializeField]
        SpriteRenderer sprite;

        [SerializeField]
        CharacterActionManager ahabActionManager;

        [SerializeField]
        float dashVelocity;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //this.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (rb.simulated)
            {
                this.transform.right = rb.linearVelocity;
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

        public void Throw(float throwForce)
        {
            this.gameObject.SetActive(true);
            this.gameObject.transform.parent = null;
            rb.simulated = true;
            rb.AddForce(this.transform.right * throwForce, ForceMode2D.Impulse);
        }

        public void SharkDash()
        {
            rb.simulated = true;
            rb.AddForce(this.transform.right * dashVelocity, ForceMode2D.Impulse);
        }

        public void PickUp(GameObject parent)
        {
            this.gameObject.SetActive(false);
            rb.simulated = false;
            this.gameObject.transform.parent = parent.transform;
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
                AhabSpecialState special = other.gameObject.GetComponentInChildren<AhabSpecialState>();
                if (special != null)
                {
                    if(special.sharkson == this)
                    {
                        PickUp(special.throwTransform.gameObject);
                    }
                }
        }
        }
    }
}
