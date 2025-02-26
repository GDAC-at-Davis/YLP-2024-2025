using State_Machine_Scripts;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.SetTransitionStates
{
    [System.Serializable]
    public class SetTransitionStatesPlayableBehavior : PlayableBehaviour
    {
        public bool isAllowed = true;
        public string[] allowedStates;
        public CharacterActionManager actionManager;

        public int flags = 0;
        public string[] states;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            actionManager = info.output.GetUserData() as CharacterActionManager;
            actionManager.SetActionTypesAllowed(isAllowed, allowedStates);

        }
    }
}
