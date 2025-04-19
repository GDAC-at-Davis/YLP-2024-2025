using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.SetTransitionStates
{
    [System.Serializable]
    public class SetTransitionStatesPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public SetTransitionStatesPlayableBehavior template = new();
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<SetTransitionStatesPlayableBehavior>.Create(graph, template);
        }
    }

}