using System;
using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween.VelocityTween
{
    [Serializable]
    public class VelocityTweenPlayableAsset : RbTweenPlayableAsset
    {
        [NoFoldOut]
        public VelocityTweenPlayableBehavior template = new();

        public new ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<VelocityTweenPlayableBehavior>.Create(graph, template);
        }
    }
}