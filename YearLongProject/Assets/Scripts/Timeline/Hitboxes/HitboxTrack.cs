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
        // Creates a runtime instance of the track, represented by a PlayableBehaviour.
        // The runtime instance performs mixing on the timeline clips.
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<HitboxTrackMixerBehavior>.Create(graph, inputCount);
        }
    }
}