using CharacterScripts;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fighters.TheCube.Scripts.SpecialTimeline
{
    // A track that allows timed restriction of changing directions
    [TrackClipType(typeof(CubeSpecialPlayableAsset))]
    [TrackBindingType(typeof(CubeSpecialHandler))]
    public class CubeSpecialTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<CubeSpecialTrackMixerBehavior>.Create(graph, inputCount);
        }
    }
}