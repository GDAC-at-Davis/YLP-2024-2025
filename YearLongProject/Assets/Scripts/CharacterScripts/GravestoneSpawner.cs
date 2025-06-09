using GameEntities;
using UnityEngine;

namespace CharacterScripts
{
    /// <summary>
    ///     Spawns a gravestone on death of a character.
    /// </summary>
    public class GravestoneSpawner : MonoBehaviour
    {
        [SerializeField]
        private Gravestone gravestonePrefab;

        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private CharacterEntity characterEntity;

        public void SpawnGravestone()
        {
            if (gravestonePrefab == null)
            {
                Debug.LogError("Gravestone prefab is not assigned.");
                return;
            }

            Gravestone gravestone = Instantiate(gravestonePrefab, spawnPoint.position, spawnPoint.rotation);
            gravestone.Initialize(characterEntity.PlayerColor);
        }
    }
}