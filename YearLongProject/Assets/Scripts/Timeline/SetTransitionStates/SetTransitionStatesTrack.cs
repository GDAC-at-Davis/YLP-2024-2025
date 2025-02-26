using State_Machine_Scripts;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.SetTransitionStates
{
    [TrackClipType(typeof(SetTransitionStatesPlayableAsset))]
    [TrackBindingType(typeof(CharacterActionManager))]
    public class SetTransitionStatesTrack : TrackAsset
    {

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SetTransitionStatesTrackMixerBehavior>.Create(graph, inputCount);
        }
    }

    public class SetTransitionStatesTrackMixerBehavior : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }
    }
}
