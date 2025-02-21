using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween.VelocityTween
{
    [Serializable]
    public class VelocityTweenPlayableAsset : RbTweenPlayableAsset
    {
        public VelocityTweenPlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<VelocityTweenPlayableBehavior>.Create(graph, template);
        }
    }
}