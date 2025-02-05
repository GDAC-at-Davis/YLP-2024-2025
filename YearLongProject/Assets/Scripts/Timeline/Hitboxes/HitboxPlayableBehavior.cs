using System;
using Hitbox.DataStructures;
using Hitbox.Emitters;
using Hitbox.HitboxAreas;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.Hitboxes
{
    /// <summary>
    ///     Hitbox runtime playable behavior
    /// </summary>
    [Serializable]
    public class HitboxPlayableBehavior : PlayableBehaviour
    {
        public BoxArea HitboxArea;
        public HitboxEffect HitEffect;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var hitboxEmitter = playerData as HitboxEmitter;

            if (hitboxEmitter == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                if (hitboxEmitter != null)
                {
                    hitboxEmitter.EmitHitbox(HitboxArea, HitEffect);
                }
            }
            else
            {
                // In edit mode, just draw the hitbox area
                HitboxContext context = hitboxEmitter.GetContext();

                HitboxArea.DrawAreaDebug(context, new DrawDebugConfig
                {
                    Color = Color.red,
                    Duration = 0
                });
            }

            base.ProcessFrame(playable, info, playerData);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            var hitboxEmitter = info.output.GetUserData() as HitboxEmitter;

            hitboxEmitter.HitEntities.Clear();

            base.OnBehaviourPlay(playable, info);
        }
    }
}