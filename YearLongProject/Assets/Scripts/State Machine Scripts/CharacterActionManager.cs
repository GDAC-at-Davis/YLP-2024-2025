using System;
using System.Collections.Generic;
using Animancer;
using Animancer.FSM;
using Input_Scripts;
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
    public Vector2 MoveDir;

    public bool JumpHeld;

    public bool DashHeld;

    public bool LightAttackHeld;

    public bool HeavyAttackHeld;

    public bool SpecialAttackHeld;
}

public class CharacterActionManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputSo playerInputSo;

    [SerializeField]
    public AnimancerComponent Anim;

    [SerializeField]
    public Character Character;

    /// <summary>
    ///     State when the Character is not currently doing any action and can freely move
    /// </summary>
    [SerializeField]
    protected MoveState MoveState;

    /// <summary>
    ///     State when the Character is jumping, exit when max height reached or when jump button is released
    /// </summary>
    [SerializeField]
    protected JumpState JumpState;

    /// <summary>
    ///     State when the Character is doing a light attack
    /// </summary>
    [SerializeField]
    protected CharacterState LightAttackState;

    /// <summary>
    ///     State when the Character is doing a heavy attack
    /// </summary>
    [SerializeField]
    protected CharacterState HeavyAttackState;

    /// <summary>
    ///     State when the Character is doing a special attack
    /// </summary>
    [SerializeField]
    protected CharacterState SpecialAttackState;

    /// <summary>
    ///     State when the Character is doing a dodge
    /// </summary>
    [SerializeField]
    protected CharacterState DashState;

    /// <summary>
    ///     State that the Character will enter upon being hit by an attack
    /// </summary>
    [SerializeField]
    protected CharacterState HitstunState;

    [SerializeField]
    private PlayerActionInput playerActionInput = new();

    protected int PlayerId => Character.PlayerId;

    public readonly StateMachine<CharacterState>.WithDefault StateMachine = new();
    private readonly float inputTimeOut = 0.5f;

    private readonly Dictionary<CharacterActionType, bool> allowedActionTypes = new();

    private StateMachine<CharacterState>.InputBuffer inputBuffer;

    protected virtual void Awake()
    {
        StateMachine.DefaultState = MoveState;
        inputBuffer = new StateMachine<CharacterState>.InputBuffer(StateMachine);
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
        if (PlayerId == -1)
        {
            return;
        }

        PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(PlayerId);
        events.MoveEvent += OnMove;
        events.JumpEvent += OnJump;
        events.DashEvent += OnDash;
        events.LightAttackEvent += OnLightAttack;
        events.HeavyAttackEvent += OnHeavyAttack;
        events.SpecialAttackEvent += OnSpecialAttack;

        StateMachine.DefaultState = MoveState;
    }

    private void OnDisable()
    {
        PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(PlayerId);
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
            Handles.Label(transform.position + Vector3.up * 10, StateMachine?.CurrentState?.ToString());
        }
#endif
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        gameObject.GetComponentInParentOrChildren(ref Anim);
        gameObject.GetComponentInParentOrChildren(ref Character);
        gameObject.GetComponentInParentOrChildren(ref MoveState);
        gameObject.GetComponentInParentOrChildren(ref JumpState);
    }
#endif

    public void Init()
    {
        // We dont subscribe in first OnEnable and do it here instead so we can use the correct ID
        PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(PlayerId);
        events.MoveEvent += OnMove;
        events.JumpEvent += OnJump;
        events.DashEvent += OnDash;
        events.LightAttackEvent += OnLightAttack;
        events.HeavyAttackEvent += OnHeavyAttack;
        events.SpecialAttackEvent += OnSpecialAttack;
    }

    private void OnMove(Vector2 moveInput)
    {
        playerActionInput.MoveDir = moveInput;
    }

    private void OnJump(bool jump)
    {
        if (!playerActionInput.JumpHeld && jump)
        {
            if (!StateMachine.TrySetState(JumpState))
            {
                inputBuffer.Buffer(JumpState, inputTimeOut);
            }
        }

        playerActionInput.JumpHeld = jump;
    }

    private void OnDash(bool dash)
    {
        playerActionInput.DashHeld = dash;
        //if (!stateMachine.TrySetState(dashState)) inputBuffer.Buffer(dashState, inputTimeOut);
    }

    private void OnLightAttack(bool attack)
    {
        playerActionInput.LightAttackHeld = attack;
        if (!StateMachine.TrySetState(LightAttackState))
        {
            inputBuffer.Buffer(LightAttackState, inputTimeOut);
        }
    }

    private void OnHeavyAttack(bool attack)
    {
        playerActionInput.HeavyAttackHeld = attack;
        //if (!stateMachine.TrySetState(heavyAttackState)) inputBuffer.Buffer(heavyAttackState, inputTimeOut);
    }

    private void OnSpecialAttack(bool attack)
    {
        playerActionInput.SpecialAttackHeld = attack;
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

    public virtual bool GetActionTypeAllowed(CharacterActionType action)
    {
        return allowedActionTypes[action];
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
        StateMachine.ForceSetState(HitstunState);
    }
}