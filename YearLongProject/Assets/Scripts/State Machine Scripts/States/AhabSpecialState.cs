using UnityEngine;
using Animancer;

namespace State_Machine_Scripts.States
{
    public class AhabSpecialState : SimpleTimelineState
    {
        [SerializeField]
        private StateNameSO heavyAttack;

        [SerializeField]
        private AhabSharkson sharkson;

        [SerializeField]
        private Transform throwTransform;

        [SerializeField]
        private float throwForce;

        public void OnEnterState()
        {
            base.OnEnterState();
        }

        protected override void HandleOnEnd()
        {
            //ActionManager.SetActionTypeAllowed(heavyAttack, false);
            //sharkson.gameObject.transform.SetPositionAndRotation(throwTransform.position, throwTransform.rotation);
            //sharkson.Throw(throwForce);
            Debug.Log("end");
            ActionManager.StateMachine.TrySetDefaultState();
        }

        public void OnExitState()
        {
            base.OnExitState();
        }
    }
}
