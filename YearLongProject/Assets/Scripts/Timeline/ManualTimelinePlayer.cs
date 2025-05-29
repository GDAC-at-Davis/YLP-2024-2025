using System;
using Animancer;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;
#if UNITY_EDITOR
using UnityEditor.Timeline;
#endif

namespace Timeline
{
    /// <summary>
    ///     A wrapper for PlayableDirector that allows manual control over playback.
    /// </summary>
    [Serializable]
    public class ManualTimelinePlayer
    {
        [SerializeField]
        private PlayableDirector playableDirector;

        [SerializeField]
        private bool loop;

        [Header("Events")]

        [InfoBox("Add listeners to define behavior when the timeline finishes playing.")]
        [SerializeField]
        private UnityEvent OnFinishedUnityEvent;

        public bool IsPlaying => playableDirector.state == PlayState.Playing;

        public event Action OnFinished;

        public void Play()
        {
            playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
            playableDirector.Play();
        }

        public void Stop()
        {
            if (playableDirector.state != PlayState.Playing)
            {
                return;
            }

            var destroyGraphOnStop = false;
#if UNITY_EDITOR
            // If we're editing a timeline, then destroy the graph so it recreates from asset
            // for Live-Editing functionality
            if (TimelineEditor.inspectedAsset != null)
            {
                destroyGraphOnStop = true;
            }
#endif

            if (destroyGraphOnStop)
            {
                playableDirector.Stop();
            }
            else
            {
                // Pause instead of stop to preserve the playableGraph
                // Slightly more performant
                playableDirector.Pause();
            }

            playableDirector.time = 0;
        }

        /// <summary>
        ///     Evaluates the PlayableDirector's time and updates its state. WARNING: This is no synchronous, and callbacks like
        ///     OnBehaviorPause will be deferred until later in the update cycle
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Evaluate(float deltaTime)
        {
            if (playableDirector.state != PlayState.Playing)
            {
                Debug.LogWarning("PlayableDirector is not playing, cannot evaluate.");
                return;
            }

            playableDirector.time += deltaTime;

            if (loop)
            {
                playableDirector.time %= playableDirector.duration;
            }

            playableDirector.Evaluate();

            if (playableDirector.time >= playableDirector.duration && !loop)
            {
                OnFinished?.Invoke();
                OnFinishedUnityEvent.Invoke();
                Stop();
            }
        }
    }
}