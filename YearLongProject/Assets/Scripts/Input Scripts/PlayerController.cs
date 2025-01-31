using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInputSO _playerInputSO;
    [SerializeField] CharacterMovementController _playerMovementController;
    int _id = -1;

    [SerializeField] Vector2 _moveInput;
    [SerializeField] bool _jump;
    [SerializeField] bool _dash;
    [SerializeField] bool _lightAttack;
    [SerializeField] bool _heavyAttack;
    [SerializeField] bool _specialAttack;

    private void OnEnable()
    {
        // When object is first instantiated OnEnable runs before Init sets the ID
        if (_id == -1) return;

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
        if (events == null) return;

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
        _playerMovementController.CharacterMove(_moveInput);
    }

    private void OnJump(bool jump)
    {
        _jump = jump;
        _playerMovementController.CharacterJump(_jump);
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
