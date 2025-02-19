using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween
{
    [Serializable]
    public abstract class RbTweenPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        /// <summary>
        ///     No blending...
        /// </summary>
        public ClipCaps clipCaps => ClipCaps.None;
    }
}