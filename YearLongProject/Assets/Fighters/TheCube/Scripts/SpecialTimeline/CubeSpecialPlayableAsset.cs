using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fighters.TheCube.Scripts.SpecialTimeline
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