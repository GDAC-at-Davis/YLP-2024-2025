using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.FastFall
{
    public class EnableFastFallPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [NoFoldOut]
        public EnableFastFallPlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<EnableFastFallPlayableBehavior>.Create(graph, template);
        }
    }
}