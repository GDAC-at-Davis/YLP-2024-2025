using EditorUtils.BoldHeader;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace GameplayUI
{
    /// <summary>
    ///     Handles creation of character HUDs when gameplay starts
    /// </summary>
    public class CharHudSpawner : MonoBehaviour
    {
        [BoldHeader("Character HUD Spawner")]
        [InfoBox("Handles creation of character HUDs when gameplay starts")]
        [Header("Depends")]

        [SerializeField]
        private GameDataSO gameDataSO;

        [SerializeField]
        private CharacterHUD characterHudPrefab;

        [SerializeField]
        private Transform hudContainer;

        private void Awake()
        {
            foreach (GameDataSO.PlayerData data in gameDataSO.AllPlayerData)
            {
                CharacterHUD characterHud = Instantiate(characterHudPrefab, hudContainer);
                characterHud.InitializeHUD(data.PlayerId);
            }
        }
    }
}