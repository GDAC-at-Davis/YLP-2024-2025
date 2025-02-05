using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    private PlayerController _PlayerController;
    private Rigidbody2D _CharacterRigidbody;

    private bool inJump;
    private bool isGrounded;
    private float playerMove;
    private float jumpVelocity;

    private void Start()
    {
        _CharacterRigidbody = GetComponent<Rigidbody2D>();
        _PlayerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, 0.55f, 3);

        float playerIntendedMove = playerMove * speed;
        float newVelocity = Mathf.Lerp(_CharacterRigidbody.linearVelocityX, playerIntendedMove, 0.2f);
        SetHorizontalVelocity(newVelocity);

        if (inJump)
        {
            SetVerticalVelocity(jumpVelocity);
        }
    }

    public void SetCharacterMove(float playerMove)
    {
        this.playerMove = playerMove;
    }

    public void SetJumpVelocity(float jumpVelocity)
    {
        this.jumpVelocity = jumpVelocity;
    }

    public void StartJump()
    {
        inJump = true;
    }

    public void StopJump()
    {
        inJump = false;
    }

    public void AddVelocity(Vector2 velocity)
    {
    }

    public void SetVelocity(Vector2 velocity)
    {
    }

    public void SetHorizontalVelocity(float velocity)
    {
        Vector3 curVel = _CharacterRigidbody.linearVelocity;
        _CharacterRigidbody.linearVelocity = new Vector2(velocity, curVel.y);
    }

    public void SetVerticalVelocity(float velocity)
    {
        Vector3 curVel = _CharacterRigidbody.linearVelocity;
        _CharacterRigidbody.linearVelocity = new Vector2(curVel.x, velocity);
    }

    public void ApplyImpulseForce(Vector2 force)
    {
        _CharacterRigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public void SetAllowMovement(bool isAllowed)
    {
    }
}