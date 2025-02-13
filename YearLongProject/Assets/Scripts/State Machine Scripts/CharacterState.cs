using Animancer;
using Animancer.FSM;
using UnityEngine;

namespace State_Machine_Scripts
{
    public class CharacterState : StateBehaviour
    {
        [Header("Base State Config")]

        [SerializeField]
        protected StateNameSO StateNameSO;

        public AnimancerComponent Anim => ActionManager.Anim;

        public string StateName => StateNameSO;

        public override bool CanEnterState
            => ActionManager.GetActionTypeAllowed(StateNameSO.Value);

        [Header("Depends")]

        protected CharacterActionManager ActionManager;

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        public void Initialize(CharacterActionManager actionManager)
        {
            ActionManager = actionManager;
        }
    }
}