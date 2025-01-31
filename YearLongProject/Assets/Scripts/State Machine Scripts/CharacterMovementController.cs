using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    private PlayerController _PlayerController;
    private Rigidbody2D _CharacterRigidbody;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float slowFallingRate = 0.25f; // Affects the jump boost from holding vs tapping jump
    bool movingLeft = false;
    bool movingRight = false;

    void Start ()
    {
        _CharacterRigidbody = GetComponent<Rigidbody2D>();
        _PlayerController = GetComponent<PlayerController>();
    }

    void FixedUpdate ()
    {
        // Make this code cleaner in the future, can't easily get playercontroller velocity on frames after the first.
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        Vector2 newVelocity = Vector2.Lerp(_CharacterRigidbody.linearVelocity, moveInput * speed, 0.2f);
        SetHorizontalVelocity(newVelocity);


        if (Input.GetKey(KeyCode.Space))
        {
            if (_CharacterRigidbody.linearVelocity.y > 0)
            {
                Vector2 newVerticalVelocity = _CharacterRigidbody.linearVelocity + new Vector2(0, slowFallingRate);
                SetVerticalVelocity(newVerticalVelocity);
            }
            
        }

    }

    public void CharacterMove(Vector2 moveInput)
    {
        // Round movement to max power if the player is holding up at the same time, otherwise enable slower movement on controller.
        if (moveInput.x > 0.7f)
        {
            Debug.Log("STARTED MOVING RIGHT");

        }
        else if (moveInput.x < -0.7f)
        {
            
            Debug.Log("STARTED MOVING LEFT");
        }
        else
        {

        }
    }

    public void CharacterJump(bool jump)
    {
        bool isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, 0.55f, 3);
        if (jump && isGrounded)
        {
            SetVerticalVelocity(new Vector2(0, jumpPower));
        }
    }

    public void AddVelocity(Vector2 velocity)
    {
    }

    public void SetHorizontalVelocity(Vector2 velocity)
    {
        Vector3 curVel = _CharacterRigidbody.linearVelocity;
        _CharacterRigidbody.linearVelocity = new Vector2 (velocity.x, curVel.y);
    }

    public void SetVerticalVelocity(Vector2 velocity)
    {
        Vector3 curVel = _CharacterRigidbody.linearVelocity;
        _CharacterRigidbody.linearVelocity = new Vector2 (curVel.x, velocity.y);
    }

    public void ApplyImpulseForce(Vector2 force)
    {
        _CharacterRigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public bool isGrounded()
    {
        return true;
    }

    public void SetAllowMovement(bool isAllowed)
    {

    }

    public void SetAllowRotation(bool isAllowed)
    {

    }
}
