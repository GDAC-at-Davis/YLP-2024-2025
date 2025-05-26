using System;
using System.Collections.Generic;
using System.Linq;
using CharacterScripts;
using EditorUtils.BoldHeader;
using GameEntities;
using LevelScripts;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    ///     Different types of player data changes
    /// </summary>
    public enum PlayerDataChange
    {
        PlayerAdded,
        PlayerRemoved,
        SelectedCharacterChanged,
        ProspectCharacterChanged,
        CharacterEntityChanged
    }

    /// <summary>
    ///     Holds global game data
    /// </summary>
    [CreateAssetMenu(fileName = "GameDataSO", menuName = "GameDataSO")]
    public class GameDataSO : ScriptableObject
    {
        public delegate void PlayerDataChangedEvent(int priorId, PlayerDataChange changeType,
            PlayerData postChangeData);

        public delegate void AllPlayersReadyEvent();

        /// <summary>
        ///     Represents a single player
        /// </summary>
        [Serializable]
        public class PlayerData
        {
            public int PlayerId;
            public CharacterSO SelectedCharacter;
            public CharacterSO ProspectCharacter;

            /// <summary>
            ///     If in gameplay, this is the actual character entity controlled by the player
            /// </summary>
            public CharacterEntity CharacterEntity;
        }

        [BoldHeader("Global Game Data")]
        [InfoBox("Holds global game data for the game.")]
        [Header("Config")]

        [SerializeField]
        private List<Color> playerColors;

        [SerializeField]
        private int maxPlayers;

        [SerializeField]
        private int minPlayers;

        [SerializeField]
        [Scene]
        private string levelSelectScene;

        public List<Color> PlayerColors => playerColors;

        public int PlayerCount => players.Count;

        public int MaxPlayers => maxPlayers;

        public LevelSO SelectedLevel => selectedLevel;

        public IEnumerable<PlayerData> AllPlayerData => players;

        /// <summary>
        ///     Event that is called when any player data changes
        /// </summary>
        public event PlayerDataChangedEvent OnPlayerDataChanged;

        /// <summary>
        ///     Event that is called when all players are ready
        /// </summary>
        public event AllPlayersReadyEvent OnAllPlayersReady;

        /// <summary>
        ///     List of players and their data
        /// </summary>
        private readonly List<PlayerData> players = new();

        [Header("Data (Debug)")]

        [ShowNonSerializedField]
        private LevelSO selectedLevel;

        private bool Hide()
        {
            // Hack to gray out the field in the inspector
            return false;
        }

        /// <summary>
        ///     Try to create a new player and associated data
        /// </summary>
        /// <returns>the playerID of the new player</returns>
        public int TryAddPlayer()
        {
            if (players.Count >= MaxPlayers)
            {
                Debug.LogWarning("Max players reached");
                return -1;
            }

            // Find the first empty "slot"
            int openPlayerId = players.Count;
            for (var i = 0; i < players.Count; i++)
            {
                if (players[i].PlayerId != i)
                {
                    openPlayerId = i;
                }
            }

            Debug.Log($"Adding player {openPlayerId} to the game");

            players.Insert(openPlayerId, new PlayerData { PlayerId = openPlayerId });

            OnPlayerDataChanged?.Invoke(openPlayerId, PlayerDataChange.PlayerAdded, players[openPlayerId]);

            return openPlayerId;
        }

        /// <summary>
        ///     Remove a player and their data
        /// </summary>
        /// <param name="id">ID of the removed player</param>
        public void RemovePlayer(int id)
        {
            // Find the player data to remove
            PlayerData playerToRemove = players.FirstOrDefault(x => x.PlayerId == id);

            if (playerToRemove == null)
            {
                Debug.LogError($"Player ID {id} doesn't exist");
                return;
            }

            Debug.Log($"Removing player {id}");

            players.Remove(playerToRemove);
            OnPlayerDataChanged?.Invoke(id, PlayerDataChange.PlayerRemoved, playerToRemove);
        }

        /// <summary>
        ///     Gets the player data for a given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mutable player data object</returns>
        public PlayerData GetPlayerData(int id)
        {
            return players.FirstOrDefault(x => x.PlayerId == id);
        }

        /// <summary>
        ///     Sets the selected character for a given ID.
        ///     If all players have a character selected, proceed to stage select
        /// </summary>
        /// <param name="id"></param>
        /// <param name="character"></param>
        public void SetPlayerSelectedCharacter(int id, CharacterSO character)
        {
            PlayerData player = players.FirstOrDefault(item => item.PlayerId == id);
            player.SelectedCharacter = character;

            OnPlayerDataChanged?.Invoke(id, PlayerDataChange.SelectedCharacterChanged, player);

            if (players.Where(item => item.SelectedCharacter == null).Count() > 0)
            {
                return;
            }

            if (players.Count < minPlayers)
            {
                return;
            }

            OnAllPlayersReady?.Invoke();
            LoadScene(levelSelectScene);
        }

        public void SetPlayerProspectCharacter(int id, CharacterSO character)
        {
            PlayerData player = players.FirstOrDefault(item => item.PlayerId == id);
            player.ProspectCharacter = character;

            OnPlayerDataChanged?.Invoke(id, PlayerDataChange.ProspectCharacterChanged, player);
        }

        /// <summary>
        ///     Removes all players and their data
        /// </summary>
        public void ClearPlayerData()
        {
            for (var i = 0; i < players.Count; i++)
            {
                OnPlayerDataChanged?.Invoke(i, PlayerDataChange.PlayerRemoved, players[i]);
            }

            players.Clear();
        }

        public void SetSelectedLevel(LevelSO level)
        {
            selectedLevel = level;
        }

        public void LoadScene(string scene)
        {
            Debug.Log($"Loading scene {scene}");
            SceneManager.LoadScene(scene);
        }

        public void SetCharacterEntity(int id, CharacterEntity character)
        {
            PlayerData player = players.FirstOrDefault(item => item.PlayerId == id);
            if (player == null)
            {
                Debug.Log($"Player ID {id} doesn't exist");
                return;
            }

            player.CharacterEntity = character;
            OnPlayerDataChanged?.Invoke(id, PlayerDataChange.CharacterEntityChanged, player);
        }

    }
}