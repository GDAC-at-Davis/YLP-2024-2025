using Animancer;
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

        [Header("Events")]

        public UnityEvent OnStateEntered;

        public string StateName => StateNameSO;

        public StateNameSO StateNameSO => stateNameSO;

        public override bool CanEnterState
            => ActionManager.GetActionTypeAllowed(StateNameSO.Value) && stateLockoutCount <= 0;

        [Header("Depends")]

        protected CharacterActionManager ActionManager;

        /// <summary>
        ///     State lockout "counter" that prevents the state from being entered if greater than 0
        /// </summary>
        private int stateLockoutCount;

        protected virtual void OnEnable()
        {
            OnStateEntered?.Invoke();
        }

        protected virtual void OnDisable()
        {
        }

        /// <summary>
        ///     Adds a lockout to the state
        /// </summary>
        public void AddLockout()
        {
            stateLockoutCount++;
        }

        /// <summary>
        ///     Removes a lockout from the state
        /// </summary>
        public void RemoveLockout()
        {
            stateLockoutCount--;
        }

        public void Initialize(CharacterActionManager actionManager)
        {
            ActionManager = actionManager;
        }
    }
}