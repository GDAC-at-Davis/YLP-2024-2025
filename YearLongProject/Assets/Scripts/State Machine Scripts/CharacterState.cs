using Animancer;
using Animancer.FSM;
using GameEntities;
using UnityEngine;

namespace State_Machine_Scripts
{
    public class CharacterState : StateBehaviour
    {
        [SerializeField]
        protected Character Character;

        [SerializeField]
        protected CharacterActionManager ActionManager;

        [SerializeField]
        protected StateNameSO StateNameSO;

        public AnimancerComponent Anim => ActionManager.Anim;

        public string StateName => StateNameSO;

        // Uses allowedActionTypes to control if entering this state is allowed.
        public override bool CanEnterState
            => ActionManager.GetActionTypeAllowed(StateNameSO.Value);

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
}