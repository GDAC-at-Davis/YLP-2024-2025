using System;
using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Hitboxes
{
    [Serializable]
    public class HitboxPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [NoFoldOut]
        public HitboxPlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<HitboxPlayableBehavior>.Create(graph, template);
        }
    }
}