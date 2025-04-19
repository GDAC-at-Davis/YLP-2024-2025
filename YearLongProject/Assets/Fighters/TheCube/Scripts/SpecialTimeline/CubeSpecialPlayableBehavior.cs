using CharacterScripts;
using UnityEngine;
using UnityEngine.Playables;

namespace Fighters.TheCube.Scripts.SpecialTimeline
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

            if (cubeSpecial == null)
            {
                Debug.LogError("No CubeSpecialHandler found on the track");
                return;
            }

            if (cubeSpecial.canSetTrap == false)
            {
                cubeSpecial.TriggerTrap((float)playable.GetDuration());
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (cubeSpecial == null)
            {
                Debug.LogError("No CubeSpecialHandler found on the track");
                return;
            }

            if (cubeSpecial.canSetTrap)
            {
                cubeSpecial.gameObject.SetActive(true);
                cubeSpecial.SetTrap();
            }
            else if (cubeSpecial.IsTrapActive)
            {
                cubeSpecial.EndTrap();
                cubeSpecial.gameObject.SetActive(false);
            }
        }
    }
}