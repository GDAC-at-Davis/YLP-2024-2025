using EditorUtils.BoldHeader;
using FMODUnity;
using Hitbox.System;
using NaughtyAttributes;
using UnityEngine;

namespace FMOD
{
    public class LandHitSFX : MonoBehaviour
    {
        [BoldHeader("Hit Landing SFX")]
        [InfoBox(
            "Handles playing SFX when a hit from this character lands. " +
            "\nAttach this to the HitEvent on a HitboxEmitter.")]
        public char IgnoreThis;

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