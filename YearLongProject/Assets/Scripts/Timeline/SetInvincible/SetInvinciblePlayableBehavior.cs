using GameEntities;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.SetInvincible
{
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

            entity.IsInvincible = true;
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

            entity.IsInvincible = false;
        }
    }
}