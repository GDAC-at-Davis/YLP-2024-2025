using UnityEngine.Playables;

namespace Fighters.TheCube.Scripts.SpecialTimeline
{
    /// <summary>
    ///     Mixer behavior for hitbox track. Empty since hitboxes don't blend.
    /// </summary>
    public class CubeSpecialTrackMixerBehavior : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }
    }
}