using System;
using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween.GravityTween
{
    [Serializable]
    public class GravityTweenPlayableAsset : RbTweenPlayableAsset
    {
        [NoFoldOut]
        public GravityTweenPlayableBehavior template = new();

        public new ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<GravityTweenPlayableBehavior>.Create(graph, template);
        }
    }
}