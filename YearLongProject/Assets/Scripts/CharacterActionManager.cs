using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;

[System.Serializable]
public enum CharacterActionType { Move, Jump, LightAttack, HeavyAttack, Dodge, Hitstun}

public class CharacterActionManager : MonoBehaviour
{
    [SerializeField]
    public AnimancerComponent anim;

    [SerializeField]
    public Character character;

    [SerializeField]
    // State when the Character is not currently doing any action and can freely move
    protected CharacterState moveState;

    [SerializeField]
    // State when the Character is jumping, exit when max height reached or when jump button is released
    protected CharacterState jumpState;

    [SerializeField]
    // State when the Character is doing a light attack
    protected CharacterState lightAttackState;

    [SerializeField]
    // State when the Character is doing a heavy attack
    protected CharacterState heavyAttackState;

    [SerializeField]
    // State when the Character is doing a dodge
    protected CharacterState dodgeState;

    [SerializeField]
    // State that the Character will enter upon being hit by an attack
    protected CharacterState hitstunState;

    public readonly StateMachine<CharacterState>.WithDefault stateMachine = new();

    private StateMachine<CharacterState>.InputBuffer inputBuffer;
    private float inputTimeOut = 0.5f;

    public Dictionary<CharacterActionType, bool> allowedActionTypes = new();

    protected virtual void Awake()
    {
        stateMachine.DefaultState = moveState;
        allowedActionTypes.Add(CharacterActionType.Move, true);
        allowedActionTypes.Add(CharacterActionType.Jump, true);
        allowedActionTypes.Add(CharacterActionType.LightAttack, true);
        allowedActionTypes.Add(CharacterActionType.HeavyAttack, true);
        allowedActionTypes.Add(CharacterActionType.Dodge, true);
        allowedActionTypes.Add(CharacterActionType.Hitstun, true);
    }

    /*
     * Add input event callbacks for changing state here
     * 
     * Example input buffering line: 
     * if (!StateMachine.TryResetState(attackState)) inputBuffer.Buffer(attackState, inputTimeOut);
     */

    public virtual void SetActionTypeAllowed(CharacterActionType action, bool isAllowed)
    {
        allowedActionTypes[action] = isAllowed;
    }

    public virtual void SetAllActionTypeAllowed(bool b)
    {
        foreach (var key in new List<CharacterActionType>(allowedActionTypes.Keys))
        {
            allowedActionTypes[key] = b;
        }
    }

    public virtual void Hitstun()
    {
        stateMachine.ForceSetState(hitstunState);
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        gameObject.GetComponentInParentOrChildren(ref anim);
        gameObject.GetComponentInParentOrChildren(ref character);
    }
#endif
}
