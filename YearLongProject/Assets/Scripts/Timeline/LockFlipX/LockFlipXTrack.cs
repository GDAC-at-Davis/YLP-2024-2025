using CharacterScripts;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.LockFlipX
{
    // A track that allows timed restriction of changing directions
    [TrackBindingType(typeof(FlipXHandler))]
    [TrackClipType(typeof(LockFlipXPlayableAsset))]
    public class LockFlipXTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<LockFlipXTrackMixerBehavior>.Create(graph, inputCount);
        }
    }
}