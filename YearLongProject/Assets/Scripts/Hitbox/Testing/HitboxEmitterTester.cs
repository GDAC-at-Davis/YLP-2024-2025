using System;
using Base;
using Hitbox.DataStructures;
using Hitbox.Emitters;
using Hitbox.HitboxAreas;
using UnityEngine;

namespace Hitbox.Testing
{
    /// <summary>
    ///     Scuffed debug script to test hitboxes
    /// </summary>
    public class HitboxEmitterTester : DescriptionMono
    {
        public enum AreaTypes
        {
            Box,
            Raycast
        }

        [Header("Context")]

        [SerializeField]
        private AreaTypes _areaType;

        [SerializeField]
        private HitboxEmitter _hitboxEmitter;

        [Header("Box Area")]

        [SerializeField]
        private Vector2 _boxCenter;

        [SerializeField]
        private float _boxRotation;

        [SerializeField]
        private Vector2 _boxSize;

        [Header("Raycast Area")]

        [SerializeField]
        private Vector2 _raycastOrigin;

        [SerializeField]
        private float _raycastAngle;

        [SerializeField]
        private float _raycastDistance;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed");

                IHitboxArea area = null;
                switch (_areaType)
                {
                    case AreaTypes.Box:
                        area = new BoxArea(_boxCenter, _boxRotation, _boxSize);
                        break;
                    case AreaTypes.Raycast:
                        area = new RaycastArea(_raycastOrigin, _raycastAngle, _raycastDistance);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _hitboxEmitter.EmitHitbox(area, new HitboxEffect());
            }
        }

        private void OnDrawGizmos()
        {
            HitboxContext context = _hitboxEmitter.GetContext();
            IHitboxArea area = null;

            switch (_areaType)
            {
                case AreaTypes.Box:
                    area = new BoxArea(_boxCenter, _boxRotation, _boxSize);
                    break;
                case AreaTypes.Raycast:
                    area = new RaycastArea(_raycastOrigin, _raycastAngle, _raycastDistance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            area.DrawAreaDebug(context, new DrawDebugConfig(Color.green, 0f));
        }
    }
}