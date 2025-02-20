using FMODUnity;
using Hitbox.System;
using UnityEngine;

namespace FMOD
{
    public class LandHitSFX : MonoBehaviour
    {
        public void PlayLandHitSFX(HitboxInstantiateResult hitboxInstantiateResult)
        {
            EventReference hitSound = hitboxInstantiateResult.HitboxInstance.HitboxEffect.HitSound;

            if (hitSound.IsNull)
            {
                return;
            }

            RuntimeManager.PlayOneShot(hitSound,
                transform.position);
        }
    }
}