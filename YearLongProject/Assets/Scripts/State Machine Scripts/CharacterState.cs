using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;

public class CharacterState : StateBehaviour
{
    [SerializeField]
    protected Character character;

    [SerializeField]
    protected CharacterActionManager actionManager;

    [SerializeField]
    protected CharacterMovementController movementController;

    [SerializeField]
    protected ClipTransition stateAnimation;

    protected CharacterActionType actionType;

    // Uses allowedActionTypes to control if entering this state is allowed.
    public override bool CanEnterState
        => actionManager.allowedActionTypes[actionType];

    private AnimancerState currentState;

    protected virtual void OnEnable()
    {
        return;
    }

    protected virtual void OnDisable()
    {
        return;
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        gameObject.GetComponentInParentOrChildren(ref character);
        actionManager = character.actionManager;
        movementController = character.movementController;
    }
#endif
}
