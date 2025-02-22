using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween.PositionTween
{
    [Serializable]
    public class PositionTweenPlayableAsset : RbTweenPlayableAsset
    {
        public PositionTweenPlayableBehavior template = new();

        public new ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<PositionTweenPlayableBehavior>.Create(graph, template);
        }
    }
}