using System;
using System.Collections.Generic;
using Animancer;
using Animancer.FSM;
using GameEntities;
using Input_Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace State_Machine_Scripts
{
    [Serializable]
    public class PlayerActionInput
    {
        public Vector2 MoveDir;

        public bool JumpHeld;

        public bool DashHeld;

        public bool LightAttackHeld;

        public bool HeavyAttackHeld;

        public bool SpecialAttackHeld;
    }

    public class CharacterActionManager : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private PlayerInputSo playerInputSo;

        [SerializeField]
        public AnimancerComponent Anim;

        [SerializeField]
        private Character character;

        [SerializeField]
        private PlayerActionInput playerActionInput = new();

        [Header("States")]

        [SerializeField]
        private List<CharacterState> states;

        [FormerlySerializedAs("jumpState")]
        [SerializeField]
        private StateNameSO jumpStateName;

        [SerializeField]
        private StateNameSO lightAttackStateName;

        protected int PlayerId => character.PlayerId;

        public readonly StateMachine<CharacterState>.WithDefault StateMachine = new();
        private readonly float inputTimeOut = 0.5f;

        private readonly Dictionary<string, bool> allowedActionTypes = new();

        private readonly Dictionary<string, CharacterState> stateDict = new();

        private StateMachine<CharacterState>.InputBuffer inputBuffer;

        protected virtual void Awake()
        {
            if (states.Count == 0)
            {
                Debug.LogError("No states found in " + name);
            }

            StateMachine.DefaultState = states[0];
            inputBuffer = new StateMachine<CharacterState>.InputBuffer(StateMachine);

            foreach (CharacterState state in states)
            {
                allowedActionTypes.Add(state.StateName, true);
                stateDict.Add(state.StateName, state);
            }
        }

        private void Update()
        {
            inputBuffer.Update();
        }

        private void OnEnable()
        {
            // When object is first instantiated OnEnable runs before Init sets the ID
            if (PlayerId == -1)
            {
                return;
            }

            PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(PlayerId);
            events.MoveEvent += OnMove;
            events.JumpEvent += OnJump;
            events.DashEvent += OnDash;
            events.LightAttackEvent += OnLightAttack;
            events.HeavyAttackEvent += OnHeavyAttack;
            events.SpecialAttackEvent += OnSpecialAttack;
        }

        private void OnDisable()
        {
            PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(PlayerId);
            // Sometimes the PlayerInputReader removes the PlayerInputEvents before we can unsubscribe from them resulting in a NullRef 
            // Could probably be resolved by setting this to run before PlayerInputEvents in code execution order but I'd rather not mess with that 
            if (events == null)
            {
                return;
            }

            events.MoveEvent -= OnMove;
            events.JumpEvent -= OnJump;
            events.DashEvent -= OnDash;
            events.LightAttackEvent -= OnLightAttack;
            events.HeavyAttackEvent -= OnHeavyAttack;
            events.SpecialAttackEvent -= OnSpecialAttack;
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
            gameObject.GetComponentInParentOrChildren(ref character);
        }
#endif
        [ContextMenu("Find States")]
        public void FindStates()
        {
            states = new List<CharacterState>(GetComponentsInChildren<CharacterState>());
        }

        public void Init()
        {
            // We dont subscribe in first OnEnable and do it here instead so we can use the correct ID
            PlayerInputSo.PlayerInputEvents events = playerInputSo.TryGetPlayerInputEvents(PlayerId);
            events.MoveEvent += OnMove;
            events.JumpEvent += OnJump;
            events.DashEvent += OnDash;
            events.LightAttackEvent += OnLightAttack;
            events.HeavyAttackEvent += OnHeavyAttack;
            events.SpecialAttackEvent += OnSpecialAttack;
        }

        private void OnMove(Vector2 moveInput)
        {
            playerActionInput.MoveDir = moveInput;
        }

        private void OnJump(bool jump)
        {
            if (!playerActionInput.JumpHeld && jump)
            {
                CharacterState jumpState = stateDict[jumpStateName];
                if (!StateMachine.TrySetState(jumpState))
                {
                    inputBuffer.Buffer(jumpState, inputTimeOut);
                }
            }

            playerActionInput.JumpHeld = jump;
        }

        private void OnDash(bool dash)
        {
            playerActionInput.DashHeld = dash;
            //if (!stateMachine.TrySetState(dashState)) inputBuffer.Buffer(dashState, inputTimeOut);
        }

        private void OnLightAttack(bool attack)
        {
            playerActionInput.LightAttackHeld = attack;

            CharacterState lightAttackState = stateDict[lightAttackStateName];
            if (!StateMachine.TrySetState(lightAttackState))
            {
                inputBuffer.Buffer(lightAttackState, inputTimeOut);
            }
        }

        private void OnHeavyAttack(bool attack)
        {
            playerActionInput.HeavyAttackHeld = attack;
            //if (!stateMachine.TrySetState(heavyAttackState)) inputBuffer.Buffer(heavyAttackState, inputTimeOut);
        }

        private void OnSpecialAttack(bool attack)
        {
            playerActionInput.SpecialAttackHeld = attack;
            //if (!stateMachine.TrySetState(specialAttackState)) inputBuffer.Buffer(specialAttackState, inputTimeOut);
        }

        public PlayerActionInput GetPlayerActionInput()
        {
            return playerActionInput;
        }

        public virtual void SetActionTypeAllowed(string action, bool isAllowed)
        {
            allowedActionTypes[action] = isAllowed;
        }

        public virtual bool GetActionTypeAllowed(string action)
        {
            return allowedActionTypes[action];
        }

        public virtual void SetAllActionTypeAllowed(bool b)
        {
            foreach (string key in new List<string>(allowedActionTypes.Keys))
            {
                allowedActionTypes[key] = b;
            }
        }
    }
}