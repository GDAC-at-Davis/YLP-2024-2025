using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline
{
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
            playableDirector.time = 0;
            playableDirector.Play();
        }

        public void Pause()
        {
            playableDirector.Pause();
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
                Pause();
            }
        }
    }
}