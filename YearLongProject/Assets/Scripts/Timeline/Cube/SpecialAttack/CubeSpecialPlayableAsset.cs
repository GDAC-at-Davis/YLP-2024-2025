using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.CubeSpecial
{
    public class CubeSpecialPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public CubeSpecialPlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<CubeSpecialPlayableBehavior>.Create(graph, template);
        }
    }
}