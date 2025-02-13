using Hitbox.Emitters;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Hitboxes
{
    /// <summary>
    ///     Timeline track for hitboxes
    /// </summary>
    [TrackColor(1f, 0f, 0f)]
    [TrackClipType(typeof(HitboxPlayableAsset))]
    [TrackBindingType(typeof(HitboxEmitter))]
    public class HitboxTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<HitboxTrackMixerBehavior>.Create(graph, inputCount);
        }
    }
}