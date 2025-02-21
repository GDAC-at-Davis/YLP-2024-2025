using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween.GravityTween
{
    [Serializable]
    public class GravityTweenPlayableAsset : RbTweenPlayableAsset
    {
        public GravityTweenPlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<GravityTweenPlayableBehavior>.Create(graph, template);
        }
    }
}