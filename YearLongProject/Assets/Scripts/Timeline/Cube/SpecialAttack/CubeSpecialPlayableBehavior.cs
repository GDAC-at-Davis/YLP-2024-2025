using CharacterScripts;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.CubeSpecial
{
    public class CubeSpecialPlayableBehavior : PlayableBehaviour
    {
        private CubeSpecialHandler cubeSpecial;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            cubeSpecial = info.output.GetUserData() as CubeSpecialHandler;

            if (cubeSpecial.IsTrapSet == false)
            {
                cubeSpecial.gameObject.SetActive(true);
                cubeSpecial.SetTrap();
            }
            else
            {
                cubeSpecial.TriggerTrap((float)(playable.GetDuration()));
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (cubeSpecial.IsTrapActive)
            {
                cubeSpecial.EndTrap();
                cubeSpecial.gameObject.SetActive(false);
            }
        }
    }
}