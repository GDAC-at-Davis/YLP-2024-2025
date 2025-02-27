using System;
using UnityEditor.Timeline;
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
        [SerializeField]
        private PlayableDirector playableDirector;

        [SerializeField]
        private bool loop;

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