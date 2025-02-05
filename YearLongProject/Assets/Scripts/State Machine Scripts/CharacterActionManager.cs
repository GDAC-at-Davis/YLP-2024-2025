using System;
using System.Collections.Generic;
using Animancer;
using Animancer.FSM;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum CharacterActionType
{
    Move,
    Jump,
    LightAttack,
    HeavyAttack,
    SpecialAttack,
    Dash,
    Hitstun
}

[Serializable]
public class PlayerActionInput
{
    public Vector2 moveDir;
    public bool jumpHeld;
    public bool dashHeld;
    public bool lightAttackHeld;
    public bool heavyAttackHeld;
    public bool specialAttackHeld;
}

public class CharacterActionManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputSO playerInputSO;

    [SerializeField]
    public AnimancerComponent anim;

    [SerializeField]
    public Character character;

    [SerializeField]
    // State when the Character is not currently doing any action and can freely move
    protected MoveState moveState;

    [SerializeField]
    // State when the Character is jumping, exit when max height reached or when jump button is released
    protected JumpState jumpState;

    [SerializeField]
    // State when the Character is doing a light attack
    protected CharacterState lightAttackState;

    [SerializeField]
    // State when the Character is doing a heavy attack
    protected CharacterState heavyAttackState;

    [SerializeField]
    // State when the Character is doing a special attack
    protected CharacterState specialAttackState;

    [SerializeField]
    // State when the Character is doing a dodge
    protected CharacterState dashState;

    [SerializeField]
    // State that the Character will enter upon being hit by an attack
    protected CharacterState hitstunState;

    [SerializeField]
    private PlayerActionInput playerActionInput = new();

    public Dictionary<CharacterActionType, bool> allowedActionTypes = new();

    protected int playerId => character.playerId;

    public readonly StateMachine<CharacterState>.WithDefault stateMachine = new();
    private readonly float inputTimeOut = 0.5f;

    private StateMachine<CharacterState>.InputBuffer inputBuffer;

    protected virtual void Awake()
    {
        stateMachine.DefaultState = moveState;
        inputBuffer = new StateMachine<CharacterState>.InputBuffer(stateMachine);
        allowedActionTypes.Add(CharacterActionType.Move, true);
        allowedActionTypes.Add(CharacterActionType.Jump, true);
        allowedActionTypes.Add(CharacterActionType.LightAttack, true);
        allowedActionTypes.Add(CharacterActionType.HeavyAttack, true);
        allowedActionTypes.Add(CharacterActionType.SpecialAttack, true);
        allowedActionTypes.Add(CharacterActionType.Dash, true);
        allowedActionTypes.Add(CharacterActionType.Hitstun, true);
    }

    private void Update()
    {
        inputBuffer.Update();
    }

    private void OnEnable()
    {
        // When object is first instantiated OnEnable runs before Init sets the ID
        if (playerId == -1)
        {
            return;
        }

        PlayerInputSO.PlayerInputEvents events = playerInputSO.GetPlayerInputEvents(playerId);
        events.MoveEvent += OnMove;
        events.JumpEvent += OnJump;
        events.DashEvent += OnDash;
        events.LightAttackEvent += OnLightAttack;
        events.HeavyAttackEvent += OnHeavyAttack;
        events.SpecialAttackEvent += OnSpecialAttack;

        stateMachine.DefaultState = moveState;
    }

    private void OnDisable()
    {
        PlayerInputSO.PlayerInputEvents events = playerInputSO.GetPlayerInputEvents(playerId);
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

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            Handles.Label(transform.position + Vector3.up * 10, stateMachine.CurrentState.ToString());
        }
#endif
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        gameObject.GetComponentInParentOrChildren(ref anim);
        gameObject.GetComponentInParentOrChildren(ref character);
        gameObject.GetComponentInParentOrChildren(ref moveState);
        gameObject.GetComponentInParentOrChildren(ref jumpState);
    }
#endif

    public void Init()
    {
        // We dont subscribe in first OnEnable and do it here instead so we can use the correct ID
        PlayerInputSO.PlayerInputEvents events = playerInputSO.GetPlayerInputEvents(playerId);
        events.MoveEvent += OnMove;
        events.JumpEvent += OnJump;
        events.DashEvent += OnDash;
        events.LightAttackEvent += OnLightAttack;
        events.HeavyAttackEvent += OnHeavyAttack;
        events.SpecialAttackEvent += OnSpecialAttack;
    }

    private void OnMove(Vector2 moveInput)
    {
        playerActionInput.moveDir = moveInput;
    }

    private void OnJump(bool jump)
    {
        if (!playerActionInput.jumpHeld && jump)
        {
            if (!stateMachine.TrySetState(jumpState))
            {
                inputBuffer.Buffer(jumpState, inputTimeOut);
            }
        }

        playerActionInput.jumpHeld = jump;
    }

    private void OnDash(bool dash)
    {
        playerActionInput.dashHeld = dash;
        //if (!stateMachine.TrySetState(dashState)) inputBuffer.Buffer(dashState, inputTimeOut);
    }

    private void OnLightAttack(bool attack)
    {
        playerActionInput.lightAttackHeld = attack;
        if (!stateMachine.TrySetState(lightAttackState))
        {
            inputBuffer.Buffer(lightAttackState, inputTimeOut);
        }
    }

    private void OnHeavyAttack(bool attack)
    {
        playerActionInput.heavyAttackHeld = attack;
        //if (!stateMachine.TrySetState(heavyAttackState)) inputBuffer.Buffer(heavyAttackState, inputTimeOut);
    }

    private void OnSpecialAttack(bool attack)
    {
        playerActionInput.specialAttackHeld = attack;
        //if (!stateMachine.TrySetState(specialAttackState)) inputBuffer.Buffer(specialAttackState, inputTimeOut);
    }

    public PlayerActionInput GetPlayerActionInput()
    {
        return playerActionInput;
    }

    public virtual void SetActionTypeAllowed(CharacterActionType action, bool isAllowed)
    {
        allowedActionTypes[action] = isAllowed;
    }

    public virtual void SetAllActionTypeAllowed(bool b)
    {
        foreach (CharacterActionType key in new List<CharacterActionType>(allowedActionTypes.Keys))
        {
            allowedActionTypes[key] = b;
        }
    }

    public virtual void Hitstun()
    {
        stateMachine.ForceSetState(hitstunState);
    }
}