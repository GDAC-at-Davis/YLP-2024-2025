using Base;
using CharacterScripts;
using Hitbox.System;
using UnityEngine;

namespace Camera
{
    /// <summary>
    ///     Script for emitting screen shake events on an entity
    /// </summary>
    public class EntityScreenShakeEmitter : DescriptionMono
    {
        [SerializeField]
        private FlipXHandler flipXHandler;

        public void ShakeOnLandHit(HitboxInstantiateResult hitboxInstantiateResult)
        {
            ScreenShakeSO effect = hitboxInstantiateResult.HitboxInstance.HitboxEffect.ScreenShakeEffect;

            Vector2 velocity = effect.Velocity;

            // Flip the X velocity if the effect is set to do so and a FlipXHandler is provided
            if (effect.FlipXVelocity && flipXHandler != null)
            {
                velocity.x *= flipXHandler.CurrentFlipX ? -1 : 1;
            }

            // Use the main camera's position as the source to avoid any falloff from distance; this keeps things simple
            UnityEngine.Camera cam = UnityEngine.Camera.main;
            if (cam != null)
            {
                effect.ImpulseDefinition.CreateEvent(cam.transform.position, velocity);
            }
        }
    }
}