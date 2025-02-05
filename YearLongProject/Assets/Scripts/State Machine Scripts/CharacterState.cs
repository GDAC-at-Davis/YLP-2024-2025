using Animancer;
using Animancer.FSM;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterState : StateBehaviour
{
    [FormerlySerializedAs("character")]
    [SerializeField]
    protected Character Character;

    [FormerlySerializedAs("actionManager")]
    [SerializeField]
    protected CharacterActionManager ActionManager;

    [FormerlySerializedAs("movementController")]
    [SerializeField]
    protected CharacterMovementController MovementController;

    [FormerlySerializedAs("stateAnimation")]
    [SerializeField]
    protected ClipTransition StateAnimation;

    // Uses allowedActionTypes to control if entering this state is allowed.
    public override bool CanEnterState
        => ActionManager.GetActionTypeAllowed(ActionType);

    protected CharacterActionType ActionType;

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

        gameObject.GetComponentInParentOrChildren(ref Character);
        ActionManager = Character.ActionManager;
        MovementController = Character.MovementController;
    }
#endif
}