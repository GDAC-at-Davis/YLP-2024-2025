using System.Collections.Generic;
using Animancer;
using Animancer.FSM;
using Base;
using Input_Scripts;
using UnityEditor;
using UnityEngine;

namespace State_Machine_Scripts
{
    public class CharacterActionManager : DescriptionMono
    {
        [Header("Depends")]

        [SerializeField]
        public AnimancerComponent Anim;

        [Header("States")]

        [Tooltip("All the states in the state machine")]
        [SerializeField]
        private List<CharacterState> states;

        public CharacterActionInput CharacterActionInput => characterActionInput;

        public readonly StateMachine<CharacterState>.WithDefault StateMachine = new();

        private readonly float inputTimeOut = 0.5f;

        /// <summary>
        ///     Dict controlling if a state is allowed to be entered
        /// </summary>
        private readonly Dictionary<string, bool> allowedStatesToEnter = new();

        /// <summary>
        ///     Dict matching state names to the actual state object
        /// </summary>
        private readonly Dictionary<string, CharacterState> stateDict = new();

        private StateMachine<CharacterState>.InputBuffer stateInputBuffer;

        private CharacterActionInput characterActionInput;

        private void Update()
        {
            stateInputBuffer.Update();
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Handles.Label(transform.position + Vector3.up * 10, StateMachine?.CurrentState.StateName);
            }
#endif
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref Anim);
        }
#endif

        public void Initialize(CharacterActionInput input)
        {
            characterActionInput = input;
            SetupStates();
        }

        private void SetupStates()
        {
            if (states.Count == 0)
            {
                Debug.LogError("No states found in " + name);
            }

            stateInputBuffer = new StateMachine<CharacterState>.InputBuffer(StateMachine);

            foreach (CharacterState state in states)
            {
                allowedStatesToEnter.Add(state.StateName, true);
                stateDict.Add(state.StateName, state);

                state.Initialize(this);
            }

            StateMachine.DefaultState = states[0];
        }

        /// <summary>
        ///     Find all states in children. Small helper tool
        /// </summary>
        [ContextMenu("Find States")]
        private void FindStates()
        {
            states = new List<CharacterState>(GetComponentsInChildren<CharacterState>());
        }

        /// <summary>
        ///     Set the state of the character, buffer if unable to set immediately
        /// </summary>
        /// <param name="stateName"></param>
        public void SetState(string stateName)
        {
            CharacterState state = stateDict.GetValueOrDefault(stateName);

            if (state == default)
            {
                Debug.LogError("State not found: " + stateName);
                return;
            }

            if (!StateMachine.TrySetState(state))
            {
                stateInputBuffer.Buffer(state, inputTimeOut);
            }
        }

        /// <inheritdoc cref="SetState(string)" />
        public void SetState(StateNameSO stateName)
        {
            SetState(stateName.Value);
        }

        public virtual void SetActionTypeAllowed(string action, bool isAllowed)
        {
            allowedStatesToEnter[action] = isAllowed;
        }

        public virtual bool GetActionTypeAllowed(string action)
        {
            return allowedStatesToEnter[action];
        }

        public virtual void SetAllActionTypeAllowed(bool b)
        {
            foreach (string key in new List<string>(allowedStatesToEnter.Keys))
            {
                allowedStatesToEnter[key] = b;
            }
        }
    }
}