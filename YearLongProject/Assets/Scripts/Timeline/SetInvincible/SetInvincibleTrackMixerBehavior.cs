using UnityEngine.Playables;

namespace Timeline.SetInvincible
{
    /// <summary>
    ///     Mixer behavior for hitbox track. Empty since hitboxes don't blend.
    /// </summary>
    public class SetInvincibleTrackMixerBehavior : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }
    }
}