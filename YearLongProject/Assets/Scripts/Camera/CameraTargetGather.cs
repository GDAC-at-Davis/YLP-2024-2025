using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camera
{
    public class CameraTargetGather : MonoBehaviour
    {
        [SerializeField]
        private FightingCamera fightingCamera;

        private void Start()
        {
            Gather();
        }

        private void Update()
        {
            if (Time.frameCount % 60 == 0)
            {
                Gather();
            }
        }

        private void Gather()
        {
            // TODO: Move to using events instead of polling every second

            IEnumerable<GameObject> targets = FindObjectsByType<CameraFramingTarget>(FindObjectsSortMode.None)
                .ToList()
                .Where(a => a.IsTargeted)
                .Select(a => a.gameObject);
            fightingCamera.SetTargets(targets);
        }
    }
}