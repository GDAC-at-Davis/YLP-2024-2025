using UnityEngine.Playables;

namespace Timeline.Hitboxes
{
    /// <summary>
    ///     Runtime behavior for the Hitbox track
    /// </summary>
    public class HitboxTrackMixerBehavior : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }
    }
}