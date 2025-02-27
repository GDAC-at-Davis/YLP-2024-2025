using Animancer.FSM;
using UnityEngine;

namespace State_Machine_Scripts
{
    public class CharacterState : StateBehaviour
    {
        [Header("Base State Config")]

        [SerializeField]
        private StateNameSO stateNameSO;

        public string StateName => StateNameSO;

        public StateNameSO StateNameSO => stateNameSO;

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