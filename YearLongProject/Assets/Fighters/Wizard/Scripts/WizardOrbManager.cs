using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CharacterScripts;
using GameEntities;

namespace Fighters.Wizard.Scripts
{
    public class WizardOrbManager : MonoBehaviour
    {
        [SerializeField]
        private int maxOrbCount = 3;
        private List<WizardOrbBehavior> currentOrbs = new();

        private bool firstSpawn = true;

        [SerializeField]
        private WizardOrbBehavior orbPrefab;
        [SerializeField]
        public Entity Wizard;

        [SerializeField]
        private CharacterFacingDirection flipX;

        private void Awake()
        {
            for (int i = 0; i < maxOrbCount; i++)
            {
                WizardOrbBehavior orb = Instantiate(orbPrefab);
                orb.Initialize(this);
                currentOrbs.Add(orb);
            }
            gameObject.SetActive(false);

            flipX.OnFlipXChange.AddListener(OnFlip);
        }

        private void OnDestroy()
        {
            flipX.OnFlipXChange.RemoveListener(OnFlip);
        }

        private void OnFlip(bool flipX)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void OnEnable()
        {
            bool spawned = false;
            foreach (WizardOrbBehavior orb in currentOrbs)
            {
                if (orb.gameObject.activeSelf) continue;

                orb.transform.position = transform.position;
                orb.gameObject.SetActive(true);
                spawned = true;
                break;
            }

            if (!spawned)
            {
                WizardOrbBehavior oldestOrb = currentOrbs[0];
                currentOrbs.Remove(oldestOrb);

                oldestOrb.transform.position = transform.position;
                oldestOrb.gameObject.SetActive(true);
                currentOrbs.Add(oldestOrb);
            }

            gameObject.SetActive(false);
        }
    }
}