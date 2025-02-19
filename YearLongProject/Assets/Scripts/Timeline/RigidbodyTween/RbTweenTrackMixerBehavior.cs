using Movement;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.RigidbodyTween
{
    /// <summary>
    ///     Mixer behavior for hitbox track. Empty since hitboxes don't blend.
    /// </summary>
    public class RbTweenTrackMixerBehavior : PlayableBehaviour
    {
        /// <summary>
        ///     This is used to reset the position on every loop when playing repeatedly in the editor.
        /// </summary>
        private Vector2 startPosition;

        private CharacterRigidbody2D rigidbody;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            rigidbody = info.output.GetUserData() as CharacterRigidbody2D;

            if (rigidbody == null)
            {
                return;
            }

            startPosition = rigidbody.transform.position;

            base.OnBehaviourPlay(playable, info);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (rigidbody == null)
            {
                return;
            }

            rigidbody.transform.position = startPosition;
        }
    }
}