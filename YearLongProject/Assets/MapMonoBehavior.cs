using EditorUtils.BoldHeader;
using Managers;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace LevelScripts
{
    public class MapMonoBehavior : MonoBehaviour
    {
        [BoldHeader("Map Data Script")]
        [InfoBox("Holds info for the map like spawnpoints")]

        [SerializeField]
        Vector3[] spawnpoints;
        public Vector3[] Spawnpoints => spawnpoints;

        [Header("Debug")]
        [SerializeField]
        [Tooltip("Width of spawn marker")]
        float markerSize;
        [SerializeField]
        [Tooltip("To color the spawnpoints")]
        GameDataSO gameDataSO;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < spawnpoints.Length; i++)
            {
                Gizmos.color = gameDataSO.PlayerColors[i];
                Gizmos.DrawSphere(spawnpoints[i], markerSize);
            }
        }

        private void OnValidate()
        {
            int max = gameDataSO.MaxPlayers;
            if (spawnpoints.Length == max)
            {
                return;
            }

            Debug.LogWarning($"There should be {max} spawnpoints!");
            Array.Resize(ref spawnpoints, max);
        }
    }
}
