using Animancer.FSM;
using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace State_Machine_Scripts
{
    public class CharacterState : StateBehaviour
    {
        [BoldHeader("Character State")]
        [InfoBox(
            "A character state. \n Make sure to add the state to the CharacterActionManager, and to define some way this state can be entered.")]
        [Header("State Info")]

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