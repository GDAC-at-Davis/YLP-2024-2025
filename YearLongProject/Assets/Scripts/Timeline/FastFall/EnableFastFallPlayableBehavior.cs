using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.FastFall
{
    [Serializable]
    public class EnableFastFallPlayableBehavior : PlayableBehaviour
    {
        private Movement.FastFall fastFall;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            fastFall = info.output.GetUserData() as Movement.FastFall;

            if (fastFall == null)
            {
                return;
            }

            fastFall.SetFastFallEnabled(true);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (fastFall == null)
            {
                return;
            }

            fastFall.SetFastFallEnabled(false);
        }
    }
}