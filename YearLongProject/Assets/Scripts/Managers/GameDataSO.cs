using System;
using System.Collections.Generic;
using EditorUtils.BoldHeader;
using LevelScripts;
using NaughtyAttributes;
using UnityEngine;

namespace Managers
{
    public enum PlayerDataChange
    {
        IdChanged,
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
        [EnableIf("Hide")]
        private LevelSO selectedLevel;

        [SerializeField]
        [EnableIf("Hide")]
        private List<PlayerData> players = new();

        public int PlayerCount => players.Count;

        public int MaxPlayers => maxPlayers;

        public LevelSO SelectedLevel => selectedLevel;

        public event PlayerDataChangedEvent OnPlayerDataChanged;

        private bool Hide()
        {
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
                Debug.LogError("Max players reached");
                return -1;
            }

            Debug.Log($"Adding player {players.Count} to the game");

            int id = players.Count;
            players.Add(new PlayerData { PlayerId = id });

            OnPlayerDataChanged?.Invoke(id, PlayerDataChange.PlayerAdded, players[id]);

            return id;
        }

        /// <summary>
        ///     Remove a player from the list
        /// </summary>
        /// <param name="id"></param>
        public void RemovePlayer(int id)
        {
            if (id < 0 || id >= players.Count)
            {
                Debug.LogError($"Player ID {id} is out of range");
                return;
            }

            Debug.Log($"Removing player {id}");

            players.RemoveAt(id);
            OnPlayerDataChanged?.Invoke(id, PlayerDataChange.PlayerRemoved, null);

            for (int i = id; i < players.Count; i++)
            {
                players[i].PlayerId--;
                OnPlayerDataChanged?.Invoke(i + 1, PlayerDataChange.IdChanged, players[i]);
            }
        }

        /// <summary>
        ///     Gets the player data for a given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mutable player data object</returns>
        public PlayerData GetPlayerData(int id)
        {
            if (id < 0 || id >= players.Count)
            {
                Debug.LogError($"Player ID {id} doesn't exist");
                return null;
            }

            return players[id];
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