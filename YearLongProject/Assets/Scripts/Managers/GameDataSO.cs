using System;
using System.Collections.Generic;
using System.Linq;
using CharacterScripts;
using EditorUtils.BoldHeader;
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
        PlayerRemoved
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
        }

        [BoldHeader("Global Game Data")]
        [InfoBox("Holds global game data for the game.")]
        [Header("Config")]

        [SerializeField]
        private int maxPlayers;

        [Header("Data (Debug)")]

        [SerializeField]
        private LevelSO selectedLevel;

        [SerializeField]
        private List<PlayerData> players = new();

        public int PlayerCount => players.Count;

        public int MaxPlayers => maxPlayers;

        public LevelSO SelectedLevel => selectedLevel;

        /// <summary>
        ///     Event that is called when any player data changes
        /// </summary>
        public event PlayerDataChangedEvent OnPlayerDataChanged;

        public event AllPlayersReadyEvent OnAllPlayersReady;

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
            OnPlayerDataChanged?.Invoke(id, PlayerDataChange.PlayerRemoved, null);
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
            players.FirstOrDefault(item => item.PlayerId == id).SelectedCharacter = character;

            if (players.Where(item => item.SelectedCharacter == null).Count() > 0) return;

            OnAllPlayersReady?.Invoke();
            LoadScene("LevelSelect");
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
            SceneManager.LoadScene(scene);
        }
    }
}