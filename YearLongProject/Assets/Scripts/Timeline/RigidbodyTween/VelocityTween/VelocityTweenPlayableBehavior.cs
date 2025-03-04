using System;
using Movement;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.RigidbodyTween.VelocityTween
{
    /// <summary>
    ///     Tweens velocity of a Rigidbody2D.
    /// </summary>
    [Serializable]
    public class VelocityTweenPlayableBehavior : PlayableBehaviour
    {
        [Header("Velocity Tween")]

        [HorizontalLine(color: EColor.White)]
        [InfoBox(
            "Velocity tween from start to end velocity over the duration of the clip.\n" +
            "Curve controls interpolation.")]
        public Vector2 StartVelocity;

        public Vector2 EndVelocity;
        public AnimationCurve Curve;

        private CharacterRigidbody2D rigidbody;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (rigidbody == null)
            {
                return;
            }

            var time = (float)playable.GetTime();
            float t = time / (float)playable.GetDuration();
            float curveValue = Curve.Evaluate(t);
            Vector2 newVelocity = Vector2.Lerp(StartVelocity, EndVelocity, curveValue);

            if (Application.isPlaying)
            {
                rigidbody.SetVelocityWithFlipX(newVelocity);
            }

            base.ProcessFrame(playable, info, playerData);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            rigidbody = info.output.GetUserData() as CharacterRigidbody2D;

            if (rigidbody == null)
            {
                return;
            }

            base.OnBehaviourPlay(playable, info);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // Make sure the velocity leaving the timeline is consistent
            if (Application.isPlaying && rigidbody != null)
            {
                rigidbody.SetVelocityWithFlipX(EndVelocity);
            }
        }
    }
}