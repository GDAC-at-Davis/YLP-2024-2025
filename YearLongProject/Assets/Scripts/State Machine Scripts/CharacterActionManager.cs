using System.Collections.Generic;
using System.Linq;
using Animancer.FSM;
using EditorUtils.BoldHeader;
using Input_Scripts;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace State_Machine_Scripts
{
    public class CharacterActionManager : MonoBehaviour
    {
        [BoldHeader("Action Manager")]
        [InfoBox("Manages the character's state machine and set the character's state. Don't remove!")]
        [Header("Dependencies")]

        [SerializeField]
        [Tooltip("Transform representing the body of the character, i.e. the part that moves")]
        private Transform _body;

        [Header("States")]

        [InfoBox("All the states used should be added here. The first state is the default state.")]
        [SerializeField]
        private List<CharacterState> states;

        public CharacterActionInput CharacterActionInput => characterActionInput;

        public float FixedDeltaTime => Time.fixedDeltaTime * InternalFixedTimeScale;

        public float InternalFixedTimeScale { get; set; } = 1f;

        public CharacterState CurrentState => StateMachine.CurrentState;

        public readonly StateMachine<CharacterState>.WithDefault StateMachine = new();

        /// <summary>
        ///     How long to buffer input for, in frames (50 fps)
        /// </summary>
        private readonly int inputBufferDuration = 7;

        /// <summary>
        ///     Dict controlling if a state is allowed to be entered
        /// </summary>
        [ShowNonSerializedField]
        private readonly Dictionary<string, bool> allowedStatesToEnter = new();

        /// <summary>
        ///     Dict matching state names to the actual state object
        /// </summary>
        private readonly Dictionary<string, CharacterState> stateDict = new();

        private StateMachine<CharacterState>.InputBuffer stateInputBuffer;

        private CharacterActionInput characterActionInput;

        private void FixedUpdate()
        {
            stateInputBuffer?.Update();
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Application.isPlaying && StateMachine?.CurrentState != null)
            {
                Handles.Label(_body.position + Vector3.up * 3, StateMachine.CurrentState.StateName);
            }
#endif
        }

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
                stateInputBuffer.Buffer(state, inputBufferDuration * Time.fixedDeltaTime);
            }
        }

        /// <inheritdoc cref="SetState(string)" />
        public void SetState(StateNameSO stateName)
        {
            SetState(stateName.Value);
        }

        /// <inheritdoc cref="SetState(string)" />
        public void SetState(CharacterState state)
        {
            SetState(state.StateName);
        }

        public virtual void SetActionTypeAllowed(string action, bool isAllowed)
        {
            allowedStatesToEnter[action] = isAllowed;
        }

        // more user friendly than setting action types one at a time
        /// <summary>
        ///     Set whether listed actions are available for transition or not
        /// </summary>
        /// <param name="isAllowed">can the listed actions be transitioned to?</param>
        /// <param name="actions">list of actions to change</param>
        public virtual void SetActionTypesAllowed(bool isAllowed, params string[] actions)
        {
            foreach (string action in actions)
            {
                allowedStatesToEnter[action] = isAllowed;
            }
        }

        /// <inheritdoc cref="SetActionTypesAllowed(bool, string[])" />
        public virtual void SetActionTypesAllowed(bool isAllowed, params StateNameSO[] actions)
        {
            foreach (StateNameSO action in actions)
            {
                allowedStatesToEnter[action.Value] = isAllowed;
            }
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

        public StateNameSO[] GetStates()
        {
            return states.Select(item => item.StateNameSO).ToArray();
        }

        [Button("Autofill States")]
        private void GetStatesButton()
        {
            CharacterState[] foundStates = GetComponentsInChildren<CharacterState>();
            foreach (CharacterState state in foundStates)
            {
                if (!states.Contains(state))
                {
                    states.Add(state);
                }
            }
        }
    }
}