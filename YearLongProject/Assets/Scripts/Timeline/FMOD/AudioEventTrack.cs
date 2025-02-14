using System;
using System.ComponentModel;
using FMODUnity;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.FMOD
{
    /// <summary>
    ///     Reimplementation of FMOD event track to work with Animancer driven playable graphs
    /// </summary>
    [TrackColor(0.066f, 0.134f, 0.244f)]
    [TrackClipType(typeof(FMODEventPlayable))]
    [TrackBindingType(typeof(GameObject))]
    [DisplayName("Audio/Audio Event Track")]
    public class AudioEventTrack : TrackAsset
    {
        public AudioEventMixerBehaviour template = new();

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            // DIFFERS: Animancer replaces PlayableDirector, and bindings work differently.

            //var director = go.GetComponent<PlayableDirector>();
            // var trackTargetObject = director.GetGenericBinding(this) as GameObject;

            GameObject trackTargetObject = go;

            foreach (TimelineClip clip in GetClips())
            {
                var playableAsset = clip.asset as FMODEventPlayable;

                if (playableAsset)
                {
                    playableAsset.TrackTargetObject = trackTargetObject;
                    playableAsset.OwningClip = clip;
                }
            }

            ScriptPlayable<AudioEventMixerBehaviour> scriptPlayable =
                ScriptPlayable<AudioEventMixerBehaviour>.Create(graph, template, inputCount);
            return scriptPlayable;
        }
    }

    [Serializable]
    public class AudioEventMixerBehaviour : PlayableBehaviour
    {
        [Range(0, 1)]
        public float volume = 1;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
#if UNITY_EDITOR
            /*
             * Process frame is called from OnGUI() when auditioning.
             * Check playing to avoid retriggering sounds while scrubbing or repainting.
             */
            bool playing = playable.GetGraph().IsPlaying();
            if (!playing)
            {
                return;
            }
#endif //UNITY_EDITOR

            int inputCount = playable.GetInputCount();

            // DIFFERS: Animancer uses its own root playable, with its own time, so we need to get our mixer time
            var time = (float)playable.GetTime();

            for (var i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<FMODEventPlayableBehavior>)playable.GetInput(i);
                FMODEventPlayableBehavior input = inputPlayable.GetBehaviour();

                input.UpdateBehavior(time, volume);
            }
        }
    }
}