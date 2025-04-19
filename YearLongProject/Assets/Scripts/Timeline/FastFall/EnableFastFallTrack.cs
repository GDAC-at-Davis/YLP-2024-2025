using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.FastFall
{
    /// <summary>
    ///     Track for enabling fast fall.
    /// </summary>
    [TrackBindingType(typeof(Movement.FastFall))]
    [TrackClipType(typeof(EnableFastFallPlayableAsset))]
    public class EnableFastFallTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<EnableFastFallTrackMixerBehavior>.Create(graph, inputCount);
        }
    }
}