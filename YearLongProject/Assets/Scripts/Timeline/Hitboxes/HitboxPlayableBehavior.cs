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
        public HitboxEffect HitEffect;
        public bool EndHitboxGroup;

        [Tooltip(
            "ID of the hitboxgroup these hitboxes belong to. Hitboxes from the same hitbox group cannot hit an Entity more than once")]
        public string HitboxGroupId;

        public RaycastArea HitboxArea;

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
                    hitboxEmitter.EmitHitbox(HitboxArea, HitEffect, HitboxGroupId);
                }
            }
            else
            {
                // In edit mode, just draw the hitbox area
                HitboxContext context = hitboxEmitter.GetContext(HitboxGroupId);

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

            base.OnBehaviourPlay(playable, info);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (EndHitboxGroup)
            {
                var hitboxEmitter = info.output.GetUserData() as HitboxEmitter;

                if (hitboxEmitter != null)
                {
                    hitboxEmitter.EndHitboxGroup(HitboxGroupId);
                }
            }
        }
    }
}