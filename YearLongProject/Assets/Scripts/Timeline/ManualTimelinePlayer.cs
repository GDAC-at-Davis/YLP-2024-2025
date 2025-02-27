using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline
{
    /// <summary>
    ///     A wrapper for PlayableDirector that allows manual control over playback.
    /// </summary>
    [Serializable]
    public class ManualTimelinePlayer
    {
#if UNITY_EDITOR
        /// <summary>
        ///     If true, the playable graph will be destroyed when the timeline is stopped, instead of paused.
        ///     This is used to support live-editing of timelines while playing.
        /// </summary>
        public static bool DestroyGraphOnStop = true;
#else
        public static bool DestroyGraphOnStop = false;
#endif

        [SerializeField]
        private PlayableDirector playableDirector;

        [SerializeField]
        private bool loop;

        public event Action OnFinished;

        public void Play()
        {
            playableDirector.time = 0;
            playableDirector.Play();
        }

        public void Stop()
        {
            if (DestroyGraphOnStop)
            {
                playableDirector.Stop();
            }
            else
            {
                // Pause instead of stop to preserve the playableGraph
                // Slightly more performant
                playableDirector.Pause();
            }
        }

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
                Stop();
            }
        }
    }
}