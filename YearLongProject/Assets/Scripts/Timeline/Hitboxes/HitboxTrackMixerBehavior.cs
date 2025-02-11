using UnityEngine.Playables;

namespace Timeline.Hitboxes
{
    /// <summary>
    ///     Mixer behavior for hitbox track. Empty since hitboxes don't blend.
    /// </summary>
    public class HitboxTrackMixerBehavior : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }
    }
}