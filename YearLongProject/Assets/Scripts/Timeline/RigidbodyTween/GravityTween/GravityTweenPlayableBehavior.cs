using System;
using Movement;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.RigidbodyTween.GravityTween
{
    /// <summary>
    ///     Tweens gravity acceleration of a Rigidbody2D.
    /// </summary>
    [Serializable]
    public class GravityTweenPlayableBehavior : PlayableBehaviour
    {
        public Vector2 StartGravity;
        public Vector2 EndGravity;
        public bool ReturnToStartGravity;
        public AnimationCurve Curve;

        private CharacterRigidbody2D rigidbody;

        private Vector2 initialGravity;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (rigidbody == null)
            {
                return;
            }

            var time = (float)playable.GetTime();
            float t = time / (float)playable.GetDuration();
            float curveValue = Curve.Evaluate(t);
            Vector2 newGravity = Vector2.Lerp(StartGravity, EndGravity, curveValue);

            rigidbody.Gravity = newGravity;

            base.ProcessFrame(playable, info, playerData);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            rigidbody = info.output.GetUserData() as CharacterRigidbody2D;

            if (rigidbody == null)
            {
                return;
            }

            initialGravity = rigidbody.Gravity;

            base.OnBehaviourPlay(playable, info);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            rigidbody.Gravity = EndGravity;
            if (ReturnToStartGravity)
            {
                rigidbody.Gravity = initialGravity;
            }
        }
    }
}