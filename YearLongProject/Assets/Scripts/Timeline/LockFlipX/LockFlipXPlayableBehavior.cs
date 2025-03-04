using System;
using CharacterScripts;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.LockFlipX
{
    [Serializable]
    public class LockFlipXPlayableBehavior : PlayableBehaviour
    {
        private CharacterFacingDirection flipX;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            flipX = info.output.GetUserData() as CharacterFacingDirection;

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