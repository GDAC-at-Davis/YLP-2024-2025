using System;
using EditorUtils.BoldHeader;
using Movement;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.RigidbodyTween.PositionTween
{
    /// <summary>
    ///     Tweens the position of a Rigidbody2D directly, bypassing velocity
    /// </summary>
    [Serializable]
    public class PositionTweenPlayableBehavior : PlayableBehaviour
    {
        [BoldHeader("Position Tween")]
        [InfoBox(
            "Tween from start position to end position over the duration of the clip.\n" +
            "Doesn't affect or disable velocity.\n" +
            "Curve controls interpolation.")]
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
            float curveValue = Curve.Evaluate(t);
            Vector2 newLocalPosition = Vector2.Lerp(StartPosition, EndPosition, curveValue);

            if (Application.isPlaying)
            {
                Vector2 delta = newLocalPosition - initialLocalPosition;
                initialLocalPosition = newLocalPosition;
                rigidbody.MoveRelativeWithFlipX(delta);
            }
            else
            {
                // In editor mode assume we start at 0,0
                rigidbody.transform.position = newLocalPosition;
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