using System;
using State_Machine_Scripts;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Timeline.SetTransitionStates
{
    [Serializable]
    public class SetTransitionStatesPlayableBehavior : PlayableBehaviour
    {
        [FormerlySerializedAs("AllowedStates")]
        public StateNameSO[] BlockedStates = { };

        public CharacterActionManager ActionManager;

        private bool isReset;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            ActionManager = info.output.GetUserData() as CharacterActionManager;

            if (ActionManager == null)
            {
                Debug.LogWarning("No CharacterActionManager bound to this clip");
                return;
            }

            ActionManager.IncrementLockToActionTypes(BlockedStates);

            isReset = false;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (ActionManager == null)
            {
                return;
            }

            if (isReset)
            {
                return;
            }

            isReset = true;
            ActionManager.DecrementLockToActionTypes(BlockedStates);
        }
    }
}