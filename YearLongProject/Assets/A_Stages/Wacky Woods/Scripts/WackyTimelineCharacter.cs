using System;
using System.Collections.Generic;
using Hitbox.Emitters;
using Hitbox.System;
using Timeline;
using UnityEngine;
using Random = UnityEngine.Random;

namespace A_Stages.Wacky_Woods.Scripts
{
    public class WackyTimelineCharacter : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> models;

        [SerializeField]
        private BasicHitboxEmitter hitboxEmitter;

        [SerializeField]
        private Transform modelTransform;

        [SerializeField]
        private List<ManualTimelinePlayer> timelinePlayer;

        public int FacingDirection { get; private set; }

        public event Action<HitboxInstantiateResult> OnLandHit;
        public event Action OnFinishAttack;

        private ManualTimelinePlayer selectedTimeline;

        /// <summary>
        ///     Initializes the character for attack with a random model
        /// </summary>
        /// <param name="facingDirection"></param>
        public void InitializeAttack(int facingDirection)
        {
            SetFacingDirection(facingDirection);

            selectedTimeline = timelinePlayer[Random.Range(0, timelinePlayer.Count)];
            selectedTimeline.Play();
            selectedTimeline.OnFinished += HandleOnFinishAttack;
        }

        public void SelectRandomModel()
        {
            int modelIndex = Random.Range(0, models.Count);

            for (var i = 0; i < models.Count; i++)
            {
                models[i].SetActive(i == modelIndex);
            }
        }

        public void SetFacingDirection(int facingDirection)
        {
            modelTransform.localScale = new Vector3(facingDirection, 1, 1);
            hitboxEmitter.SetFlipX(facingDirection <= 0);
            FacingDirection = facingDirection;
        }

        public void StopAttacking()
        {
            if (selectedTimeline != null)
            {
                selectedTimeline.OnFinished -= HandleOnFinishAttack;
                selectedTimeline.Stop();
            }
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        private void HandleOnFinishAttack()
        {
            selectedTimeline.OnFinished -= HandleOnFinishAttack;
            OnFinishAttack?.Invoke();
        }

        public void HandleLandHit(HitboxInstantiateResult hitboxInstantiateResult)
        {
            OnLandHit?.Invoke(hitboxInstantiateResult);
        }

        public void Evaluate(float deltaTime)
        {
            if (selectedTimeline != null)
            {
                selectedTimeline.Evaluate(deltaTime);
            }
        }
    }
}