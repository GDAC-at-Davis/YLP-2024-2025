using System;
using Hitbox.HitboxAreas;
using UnityEngine;

/// <summary>
///     Shitty debug script to test hitboxes
/// </summary>
public class HitboxTester : MonoBehaviour
{
    public enum AreaTypes
    {
        Box
    }

    [SerializeField]
    private AreaTypes _areaType;

    [SerializeField]
    private LayerMask _layerMask;

    [Header("Box Area")]

    [SerializeField]
    private Vector2 _boxCenter;

    [SerializeField]
    private float _boxRotation;

    [SerializeField]
    private Vector2 _boxSize;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            var context = new HitboxCheckContext
            {
                LayerMask = _layerMask,
                SourcePosition = transform.position,
                SourceAngle = transform.rotation.eulerAngles.z
            };

            IHitboxArea area = null;

            switch (_areaType)
            {
                case AreaTypes.Box:
                    area = new BoxArea(_boxCenter, _boxRotation, _boxSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Collider2D[] colliders = area.GetCollidersInArea(context);
            area.DrawAreaDebug(context, new DrawDebugConfig(Color.red, 3f));

            foreach (Collider2D collider in colliders)
            {
                Debug.Log($"Collider: {collider.name}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        var context = new HitboxCheckContext
        {
            LayerMask = _layerMask,
            SourcePosition = transform.position,
            SourceAngle = transform.rotation.eulerAngles.z
        };

        IHitboxArea area = null;

        switch (_areaType)
        {
            case AreaTypes.Box:
                area = new BoxArea(_boxCenter, _boxRotation, _boxSize);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        area.DrawAreaDebug(context, new DrawDebugConfig(Color.green, 0f));
    }
}