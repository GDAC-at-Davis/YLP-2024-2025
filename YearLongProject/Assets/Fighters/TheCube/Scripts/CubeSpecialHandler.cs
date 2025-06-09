using Base;
using Input_Scripts;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterScripts
{
    /// <summary>
    ///     Handles flipping the character based on input
    /// </summary>
    public class CubeSpecialHandler : DescriptionMono
    {


        public UnityEvent<bool> OnTrapSetChange;

        public bool canSetTrap = true;
        public bool IsTrapActive = false;

        private Vector3 startPos;
        private Vector3 endPos;
        private float trapDuration;
        private float elapsedDuration;

        Rigidbody2D playerRigidbody;


        private void Start()
        {
            playerRigidbody = transform.parent.gameObject.GetComponentInChildren<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            if (IsTrapActive)
            {
                elapsedDuration += Time.deltaTime;

                float trapProgress = elapsedDuration / trapDuration;

                endPos = playerRigidbody.transform.position + Vector3.up * 0.5f;

                if (trapProgress < 1)
                {
                    transform.position = Vector3.Lerp(startPos, endPos, Mathf.Pow(trapProgress, 2));
                }
                
            }
        }

        public void SetTrap()
        {
            gameObject.transform.position = playerRigidbody.transform.position + Vector3.up * 0.5f;
            startPos = transform.position;
            canSetTrap = false;
        }

        public void TriggerTrap(float duration)
        {
			playerRigidbody.transform.position = gameObject.transform.position;
            gameObject.transform.position = Vector3.one * -100;
            canSetTrap = true;
			/*
            IsTrapActive = true;

            elapsedDuration = 0;
            trapDuration = duration;

            endPos = playerRigidbody.transform.position + Vector3.up * 0.5f;
            */
		}
        

        public void EndTrap()
        {
            canSetTrap = true;
            IsTrapActive = false;
			gameObject.SetActive(false);
		}

    }
}