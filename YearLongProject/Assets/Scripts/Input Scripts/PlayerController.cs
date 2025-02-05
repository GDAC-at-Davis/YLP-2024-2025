using Input_Scripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputSo playerInputSo;

    [SerializeField]
    private CharacterMovementController playerMovementController;

    [SerializeField]
    private Vector2 moveInput;

    [SerializeField]
    private bool jump;

    [SerializeField]
    private bool dash;

    [SerializeField]
    private bool lightAttack;

    [SerializeField]
    private bool heavyAttack;

    [SerializeField]
    private bool specialAttack;

    public Vector2 MoveInput => moveInput;

    public bool Jump => jump;
    private int id = -1;

    private void OnEnable()
    {
        // When object is first instantiated OnEnable runs before Init sets the ID
        if (id == -1)
        {
            return;
        }

        PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(id);
        events.MoveEvent += OnMove;
        events.JumpEvent += OnJump;
        events.DashEvent += OnDash;
        events.LightAttackEvent += OnLightAttack;
        events.HeavyAttackEvent += OnHeavyAttack;
        events.SpecialAttackEvent += OnSpecialAttack;
    }

    private void OnDisable()
    {
        PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(id);
        // Sometimes the PlayerInputReader removes the PlayerInputEvents before we can unsubscribe from them resulting in a NullRef 
        // Could probably be resolved by setting this to run before PlayerInputEvents in code execution order but I'd rather not mess with that 
        if (events == null)
        {
            return;
        }

        events.MoveEvent -= OnMove;
        events.JumpEvent -= OnJump;
        events.DashEvent -= OnDash;
        events.LightAttackEvent -= OnLightAttack;
        events.HeavyAttackEvent -= OnHeavyAttack;
        events.SpecialAttackEvent -= OnSpecialAttack;
    }

    public void Init(int id)
    {
        this.id = id;
        transform.parent = null;

        // We dont subscribe in first OnEnable and do it here instead so we can use the correct ID
        PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(this.id);
        events.MoveEvent += OnMove;
        events.JumpEvent += OnJump;
        events.DashEvent += OnDash;
        events.LightAttackEvent += OnLightAttack;
        events.HeavyAttackEvent += OnHeavyAttack;
        events.SpecialAttackEvent += OnSpecialAttack;
    }

    private void OnMove(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    private void OnJump(bool jump)
    {
        this.jump = jump;
    }

    private void OnDash(bool dash)
    {
        this.dash = dash;
    }

    private void OnLightAttack(bool attack)
    {
        lightAttack = attack;
    }

    private void OnHeavyAttack(bool attack)
    {
        heavyAttack = attack;
    }

    private void OnSpecialAttack(bool attack)
    {
        specialAttack = attack;
    }
}