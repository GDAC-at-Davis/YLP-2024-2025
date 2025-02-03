using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Hitboxes
{
    [Serializable]
    public class HitboxPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public HitboxPlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<HitboxPlayableBehavior>.Create(graph, template);
        }
    }
}