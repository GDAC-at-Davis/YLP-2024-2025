using UnityEngine;
using Animancer;

namespace State_Machine_Scripts.States
{
    public class AhabSpecialState : CharacterState
    {
        [SerializeField]
        private StateNameSO heavyAttack;

        [SerializeField]
        private StateNameSO specialAttack;

        [SerializeField]
        private AhabSharkson sharkson;

        [SerializeField]
        private Transform throwTransform;

        [SerializeField]
        private float throwForce;

        [Header("Config")]

        [SerializeField]
        protected SimpleMovementController movementController;

        [SerializeField]
        protected PlayableAssetTransition lightAttackPlayableAsset;

        [SerializeField]
        protected bool useDefaultMovement;

        [Tooltip("Which states can you not cancel into from this state?")]
        [SerializeField]
        protected StateNameSO[] blockedStates;

        private void Update()
        {
            if (useDefaultMovement)
            {
                Vector2 moveInput = ActionManager.CharacterActionInput.MoveInput;
                movementController.SetCharacterMove(moveInput.x);
            }
            else
            {
                movementController.SetCharacterMove(0);
            }
        }

        public override void OnEnterState()
        {
            Debug.Log("enterState");
            if (lightAttackPlayableAsset.State != null)
            {
                lightAttackPlayableAsset.State.Destroy();
            }

            Anim.Play(lightAttackPlayableAsset);
            lightAttackPlayableAsset.Events.OnEnd += HandleOnEnd;
            foreach (StateNameSO state in blockedStates)
            {
                ActionManager.SetActionTypeAllowed(state, false);
            }
        }

        protected virtual void HandleOnEnd()
        {
            ActionManager.SetActionTypeAllowed(heavyAttack, false);
            ActionManager.SetActionTypeAllowed(specialAttack, false);
            sharkson.gameObject.transform.SetPositionAndRotation(throwTransform.position, throwTransform.rotation);
            sharkson.Throw(throwForce);
            Debug.Log("handleOnEnd");
            ActionManager.StateMachine.TrySetDefaultState();
        }

        public override void OnExitState()
        {
            Debug.Log("exitState");
            lightAttackPlayableAsset.Events.OnEnd -= HandleOnEnd;
            foreach (StateNameSO state in blockedStates)
            {
                ActionManager.SetActionTypeAllowed(state, true);
            }
        }
    }
}
