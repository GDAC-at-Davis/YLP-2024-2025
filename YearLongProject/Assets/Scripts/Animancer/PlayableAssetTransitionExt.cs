using System;
using UnityEngine;

namespace Animancer
{
    [Serializable]
    public class PlayableAssetTransitionExt : PlayableAssetTransition
    {
        [SerializeField]
        private bool isLooping;

        public override bool IsLooping => isLooping;
    }
}