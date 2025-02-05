using Animancer;
using Animancer.FSM;
using UnityEngine;

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

    // Uses allowedActionTypes to control if entering this state is allowed.
    public override bool CanEnterState
        => actionManager.GetActionTypeAllowed(actionType);

    protected CharacterActionType actionType;

    private AnimancerState currentState;

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
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