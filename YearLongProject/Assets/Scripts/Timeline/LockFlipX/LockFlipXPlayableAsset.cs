using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.LockFlipX
{
    public class LockFlipXPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [NoFoldOut]
        public LockFlipXPlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<LockFlipXPlayableBehavior>.Create(graph, template);
        }
    }
}