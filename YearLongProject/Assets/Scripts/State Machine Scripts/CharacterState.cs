using Animancer;
using Animancer.FSM;
using UnityEngine;

public class CharacterState : StateBehaviour
{
    [SerializeField]
    protected Character Character;

    [SerializeField]
    protected CharacterActionManager ActionManager;

    public AnimancerComponent Anim => ActionManager.Anim;

    // Uses allowedActionTypes to control if entering this state is allowed.
    public override bool CanEnterState
        => ActionManager.GetActionTypeAllowed(ActionType);

    protected CharacterActionType ActionType;

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
    }
#endif
}