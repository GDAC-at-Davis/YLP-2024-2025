using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween
{
    [Serializable]
    public class RbTweenPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public RbTweenPlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<RbTweenPlayableBehavior>.Create(graph, template);
        }
    }
}