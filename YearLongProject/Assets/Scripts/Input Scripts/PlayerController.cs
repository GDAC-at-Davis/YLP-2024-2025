using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputSO _playerInputSO;

    [SerializeField]
    private CharacterMovementController _playerMovementController;

    [SerializeField]
    private Vector2 _moveInput;

    [SerializeField]
    private bool _jump;

    [SerializeField]
    private bool _dash;

    [SerializeField]
    private bool _lightAttack;

    [SerializeField]
    private bool _heavyAttack;

    [SerializeField]
    private bool _specialAttack;

    public Vector2 MoveInput => _moveInput;

    public bool Jump => _jump;
    private int _id = -1;

    private void OnEnable()
    {
        // When object is first instantiated OnEnable runs before Init sets the ID
        if (_id == -1)
        {
            return;
        }

        PlayerInputSO.PlayerInputEvents events = _playerInputSO.GetPlayerInputEvents(_id);
        events.MoveEvent += OnMove;
        events.JumpEvent += OnJump;
        events.DashEvent += OnDash;
        events.LightAttackEvent += OnLightAttack;
        events.HeavyAttackEvent += OnHeavyAttack;
        events.SpecialAttackEvent += OnSpecialAttack;
    }

    private void OnDisable()
    {
        PlayerInputSO.PlayerInputEvents events = _playerInputSO.GetPlayerInputEvents(_id);
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
        _id = id;
        transform.parent = null;

        // We dont subscribe in first OnEnable and do it here instead so we can use the correct ID
        PlayerInputSO.PlayerInputEvents events = _playerInputSO.GetPlayerInputEvents(_id);
        events.MoveEvent += OnMove;
        events.JumpEvent += OnJump;
        events.DashEvent += OnDash;
        events.LightAttackEvent += OnLightAttack;
        events.HeavyAttackEvent += OnHeavyAttack;
        events.SpecialAttackEvent += OnSpecialAttack;
    }

    private void OnMove(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    private void OnJump(bool jump)
    {
        _jump = jump;
    }

    private void OnDash(bool dash)
    {
        _dash = dash;
    }

    private void OnLightAttack(bool attack)
    {
        _lightAttack = attack;
    }

    private void OnHeavyAttack(bool attack)
    {
        _heavyAttack = attack;
    }

    private void OnSpecialAttack(bool attack)
    {
        _specialAttack = attack;
    }
}