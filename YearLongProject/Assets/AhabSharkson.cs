using UnityEngine;
using State_Machine_Scripts;

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
        this.gameObject.transform.parent = parent.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit ground");
        if(other.gameObject.layer == 6)
        {
            rb.simulated = false;
        }
        //else if(other.gameObject.layer == )
    }
}
