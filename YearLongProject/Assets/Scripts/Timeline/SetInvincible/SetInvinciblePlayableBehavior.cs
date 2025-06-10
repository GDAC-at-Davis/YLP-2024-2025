using System;
using GameEntities;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.SetInvincible
{
    [Serializable]
    public class SetInvinciblePlayableBehavior : PlayableBehaviour
    {
        private Entity entity;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            entity = info.output.GetUserData() as Entity;

            if (entity == null)
            {
                return;
            }

            entity.AddInvincibility();
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (entity == null)
            {
                return;
            }

            entity.RemoveInvincibility();
        }
    }
}