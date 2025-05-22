using UnityEngine;

namespace Utils
{
    public static class ExtensionMethods
    {
        public static bool IsInLayerMask(this LayerMask layerMask, Collider2D collider)
        {
            return (layerMask & (1 << collider.gameObject.layer)) != 0;
        }
    }
}