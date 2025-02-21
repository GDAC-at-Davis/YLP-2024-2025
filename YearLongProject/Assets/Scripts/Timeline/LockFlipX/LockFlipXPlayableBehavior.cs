using CharacterScripts;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.LockFlipX
{
    public class LockFlipXPlayableBehavior : PlayableBehaviour
    {
        private FlipXHandler flipX;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            flipX = info.output.GetUserData() as FlipXHandler;

            if (flipX == null)
            {
                return;
            }

            flipX.CanFlipX = false;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (flipX == null)
            {
                return;
            }

            flipX.CanFlipX = true;
        }
    }
}