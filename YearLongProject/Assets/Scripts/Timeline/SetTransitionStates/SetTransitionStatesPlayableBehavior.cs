using System;
using State_Machine_Scripts;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.SetTransitionStates
{
    [Serializable]
    public class SetTransitionStatesPlayableBehavior : PlayableBehaviour
    {
        public StateNameSO[] AllowedStates = { };
        public CharacterActionManager ActionManager;

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

            ActionManager.SetAllActionTypeAllowed(false);
            ActionManager.SetActionTypesAllowed(true, AllowedStates);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (ActionManager == null)
            {
                return;
            }

            ActionManager.SetAllActionTypeAllowed(true);
        }
    }
}