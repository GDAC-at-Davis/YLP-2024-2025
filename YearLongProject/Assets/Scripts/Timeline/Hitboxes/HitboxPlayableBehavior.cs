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

        public RaycastArea[] RaycastArea;
        public BoxArea[] BoxArea;

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
                    foreach (RaycastArea raycastArea in RaycastArea)
                    {
                        hitboxEmitter.EmitHitbox(raycastArea, HitEffect, HitboxGroupId);
                    }
                    foreach (BoxArea boxArea in BoxArea)
                    {
                        hitboxEmitter.EmitHitbox(boxArea, HitEffect, HitboxGroupId);
                    }
                }
            }
            else
            {
                // In edit mode, just draw the hitbox area
                HitboxContext context = hitboxEmitter.GetContext(HitboxGroupId);
                foreach (RaycastArea raycastArea in RaycastArea)
                {
                    raycastArea.DrawAreaDebug(context, new DrawDebugConfig
                    {
                        Color = Color.red,
                        Duration = 0
                    });
                }
                foreach (BoxArea boxArea in BoxArea)
                {
                    boxArea.DrawAreaDebug(context, new DrawDebugConfig
                    {
                        Color = Color.red,
                        Duration = 0
                    });
                }
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