using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CharacterScripts;
using GameEntities;
using Hitbox.DataStructures;
using Hitbox.System;
using System;
using State_Machine_Scripts.States;

namespace Fighters.Gardener.Scripts
{
    public class GardenerThornManager : MonoBehaviour
    {
        // Workaround for Activation track and object pooling design pattern for orbs
        private bool firstSpawn = true;

        [SerializeField]
        private GardenerThornBehavior thornPrefab;
        private GardenerThornBehavior currentThorn;
        [SerializeField]
        public CharacterEntity Gardener;

        [SerializeField]
        private CharacterFacingDirection flipX;

        [SerializeField]
        SimpleTimelineState projectile;
        [SerializeField]
        SimpleTimelineState teleport;

        private void Awake()
        {
            GardenerThornBehavior thorn = Instantiate(thornPrefab);
            thorn.Initialize(this);
            currentThorn = thorn;
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

        public void SpecialState()
        {
            if (!currentThorn.gameObject.activeSelf)
            {
                Gardener.ActionManager.SetState(projectile);
            }
            if (currentThorn.Attached)
            {
                Gardener.ActionManager.SetState(teleport);
            }
        }

        // Timeline track only supports enabling and not instantiating so whenever this is enabled it'll spawn an orb
        private void OnEnable()
        {
            if (!currentThorn.gameObject.activeSelf)
            {
                SpawnThorn(currentThorn);
            }

            // if thorn attached to something deactivate and teleport to it
            if (currentThorn.Attached)
            {
                Gardener.MovementController.CharacterRigidbody.transform.position = (Vector2)currentThorn.transform.position;
                currentThorn.Warp();
            }

            gameObject.SetActive(false);
        }

        // spawn thorn in front of gardener, on top if there is no available space to place thorn
        private void SpawnThorn(GardenerThornBehavior thorn)
        {
            thorn.transform.position = Gardener.MovementController.Collider.bounds.center;
            thorn.Direction = (int)Mathf.Sign(transform.localScale.x);
            thorn.gameObject.SetActive(true);
        }
    }
}
