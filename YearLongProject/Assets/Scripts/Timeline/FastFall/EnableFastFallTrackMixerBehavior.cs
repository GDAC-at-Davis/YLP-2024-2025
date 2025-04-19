using UnityEngine.Playables;

namespace Timeline.FastFall
{
    /// <summary>
    ///     Mixer behavior for hitbox track. Empty since hitboxes don't blend.
    /// </summary>
    public class EnableFastFallTrackMixerBehavior : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }
    }
}