using Hitbox.System;
using UnityEngine;

namespace Camera
{
    /// <summary>
    ///     Script for emitting screen shake events on an entity
    /// </summary>
    public class EntityScreenShakeEmitter : MonoBehaviour
    {
        public void ShakeOnLandHit(HitboxInstantiateResult hitboxInstantiateResult)
        {
            ScreenShakeSO effect = hitboxInstantiateResult.HitboxInstance.HitboxEffect.ScreenShakeEffect;

            // Use the main camera's position as the source to avoid any falloff from distance; this keeps things simple
            UnityEngine.Camera cam = UnityEngine.Camera.main;
            if (cam != null)
            {
                effect.ImpulseDefinition.CreateEvent(cam.transform.position, effect.Velocity);
            }
        }
    }
}