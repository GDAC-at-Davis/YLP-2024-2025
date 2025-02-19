using Base;
using UnityEngine;

namespace Animancer
{
    public class SetUpdateMode : DescriptionMono
    {
        [SerializeField]
        private AnimancerComponent animancer;

        [SerializeField]
        private AnimatorUpdateMode updateMode;

        private void Awake()
        {
            animancer.UpdateMode = updateMode;
        }
    }
}