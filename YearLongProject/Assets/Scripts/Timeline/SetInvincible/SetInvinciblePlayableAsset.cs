using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.SetInvincible
{
    public class SetInvinciblePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public SetInvinciblePlayableBehavior template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<SetInvinciblePlayableBehavior>.Create(graph, template);
        }
    }
}