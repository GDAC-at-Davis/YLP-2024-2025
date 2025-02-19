using System;
using Movement;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.RigidbodyTween
{
    /// <summary>
    ///     Hitbox runtime playable behavior
    /// </summary>
    [Serializable]
    public class RbTweenPlayableBehavior : PlayableBehaviour
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public AnimationCurve Curve;

        private Vector2 initialLocalPosition;
        private CharacterRigidbody2D rigidbody;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (rigidbody == null)
            {
                return;
            }

            var time = (float)playable.GetTime();
            float t = time / (float)playable.GetDuration();

            if (Application.isPlaying)
            {
                Vector2 newLocalPosition = Vector2.Lerp(StartPosition, EndPosition, Curve.Evaluate(t));
                Vector2 delta = newLocalPosition - initialLocalPosition;
                initialLocalPosition = newLocalPosition;
                rigidbody.MoveRelativeWithFlipX(delta);
            }
            else
            {
                // In editor mode assume we start at 0,0
                Vector2 newPosition = Vector2.Lerp(StartPosition, EndPosition, Curve.Evaluate(t));
                rigidbody.transform.position = newPosition;
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

            initialLocalPosition = StartPosition;

            base.OnBehaviourPlay(playable, info);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (Application.isPlaying && rigidbody != null)
            {
                Vector2 newLocalPosition = EndPosition;
                Vector2 delta = newLocalPosition - initialLocalPosition;
                initialLocalPosition = newLocalPosition;
                rigidbody.MoveRelativeWithFlipX(delta);
            }
        }
    }
}