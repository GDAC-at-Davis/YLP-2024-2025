using GameEntities;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.SetInvincible
{
    // A track that allows timed restriction of changing directions
    [TrackBindingType(typeof(Entity))]
    [TrackClipType(typeof(SetInvinciblePlayableAsset))]
    public class SetInvincibleTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SetInvincibleTrackMixerBehavior>.Create(graph, inputCount);
        }
    }
}