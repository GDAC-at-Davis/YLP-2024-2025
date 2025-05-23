using System;
using EditorUtils.BoldHeader;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace LevelScripts
{
    public class MapMonoBehavior : MonoBehaviour
    {
        [BoldHeader("Map Data Script")]
        [InfoBox("Holds info for the map like spawnpoints")]
        [SerializeField]
        private Vector3[] spawnpoints;

        [Header("Debug")]

        [SerializeField]
        [Tooltip("Width of spawn marker")]
        private float markerSize;

        [SerializeField]
        [Tooltip("To color the spawnpoints")]
        private GameDataSO gameDataSO;

        public Vector3[] Spawnpoints => spawnpoints;

        private void OnDrawGizmos()
        {
            for (var i = 0; i < spawnpoints.Length; i++)
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