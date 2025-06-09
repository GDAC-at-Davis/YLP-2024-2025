using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CharacterScripts;
using GameEntities;
using Hitbox.DataStructures;
using Hitbox.System;
using System;

namespace Fighters.Wizard.Scripts
{
    // Workaround for Activation track and object pooling design pattern for orbs
    public class WizardOrbManager : MonoBehaviour
    {
        [SerializeField]
        private int maxOrbCount = 3;
        private List<WizardOrbBehavior> currentOrbs = new();

        private bool firstSpawn = true;

        [SerializeField]
        private WizardOrbBehavior orbPrefab;
        [SerializeField]
        public CharacterEntity Wizard;

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
            Wizard.OnHitByAttackEvent.AddListener(OnHit);
        }

        // Destroy orbs when wizard is hit
        private void OnHit(HitboxInstance instance, HitImpact impact)
        {
            foreach (WizardOrbBehavior orb in new List<WizardOrbBehavior>(currentOrbs))
            {
                orb.DestroyBall();
            }
        }

        private void OnDestroy()
        {
            flipX.OnFlipXChange.RemoveListener(OnFlip);
            Wizard.OnHitByAttackEvent.RemoveListener(OnHit);
        }

        private void OnFlip(bool flipX)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        // Timeline track only supports enabling and not instantiating so whenever this is enabled it'll spawn an orb
        private void OnEnable()
        {
            bool spawned = false;
            foreach (WizardOrbBehavior orb in currentOrbs)
            {
                if (orb.gameObject.activeSelf) continue;

                SpawnOrb(orb);
                spawned = true;
                break;
            }

            // if at max active orbs, destroy oldest orb and spawn new orb
            if (!spawned)
            {
                WizardOrbBehavior oldestOrb = currentOrbs[0];
                currentOrbs.Remove(oldestOrb);

                SpawnOrb(oldestOrb);
                currentOrbs.Add(oldestOrb);
            }

            gameObject.SetActive(false);
        }

        // maintain list to track oldest orb
        public void ResetOrb(WizardOrbBehavior orb)
        {
            currentOrbs.Remove(orb);
            currentOrbs.Add(orb);
        }

        // spawn orb in front of wizard, on top if there is no available space to place orb
        private void SpawnOrb(WizardOrbBehavior orb)
        {
            orb.transform.position = transform.position;
            if (Physics2D.BoxCast(transform.position, orb.Collider.size, 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Terrain")))
            {
                orb.transform.position = Wizard.MovementController.Collider.bounds.center + (Vector3.back * 0.1f);
            }
            orb.Reset();
            orb.gameObject.SetActive(true);
        }
    }
}