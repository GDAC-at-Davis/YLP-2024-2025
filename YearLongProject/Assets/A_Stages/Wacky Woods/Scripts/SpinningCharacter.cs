using System;
using System.Collections.Generic;
using Hitbox.Emitters;
using Hitbox.System;
using Timeline;
using UnityEngine;
using Random = UnityEngine.Random;

namespace A_Stages.Wacky_Woods.Scripts
{
    public class SpinningCharacter : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> models;

        [SerializeField]
        private BasicHitboxEmitter hitboxEmitter;

        [SerializeField]
        private Transform modelTransform;

        [SerializeField]
        private ManualTimelinePlayer timelinePlayer;

        public event Action<HitboxInstantiateResult> OnLandHit;

        /// <summary>
        ///     Initializes the character for spinning with a random model
        /// </summary>
        /// <param name="facingDirection"></param>
        public void InitializeSpinning(int facingDirection)
        {
            SetFacingDirection(facingDirection);

            int modelIndex = Random.Range(0, models.Count);

            for (var i = 0; i < models.Count; i++)
            {
                models[i].SetActive(i == modelIndex);
            }

            timelinePlayer.Play();
        }

        public void SetFacingDirection(int facingDirection)
        {
            modelTransform.localScale = new Vector3(facingDirection, 1, 1);
            hitboxEmitter.SetFlipX(facingDirection <= 0);
        }

        public void StopSpinning()
        {
            timelinePlayer.Stop();

            foreach (GameObject model in models)
            {
                model.SetActive(false);
            }
        }

        public void HandleLandHit(HitboxInstantiateResult hitboxInstantiateResult)
        {
            OnLandHit?.Invoke(hitboxInstantiateResult);
        }

        public void Evaluate(float deltaTime)
        {
            timelinePlayer.Evaluate(deltaTime);
        }
    }
}