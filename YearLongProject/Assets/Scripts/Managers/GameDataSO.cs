using System;
using System.Collections.Generic;
using System.Linq;
using EditorUtils.BoldHeader;
using LevelScripts;
using NaughtyAttributes;
using UnityEngine;

namespace Managers
{
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

        /// <summary>
        ///     Represents a single player
        /// </summary>
        [Serializable]
        public class PlayerData
        {
            public int PlayerId;
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

        private bool Hide()
        {
            // Hack to gray out the field in the inspector
            return false;
        }

        /// <summary>
        ///     Try to create a new "player"
        /// </summary>
        /// <returns></returns>
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
        ///     Remove a player from the list
        /// </summary>
        /// <param name="id"></param>
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

        public void ClearPlayerData()
        {
            for (var i = 0; i < players.Count; i++)
            {
                OnPlayerDataChanged?.Invoke(i, PlayerDataChange.PlayerRemoved, players[i]);
            }

            players.Clear();
        }
    }
}