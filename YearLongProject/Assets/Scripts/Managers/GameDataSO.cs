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
        public delegate void OnPlayerDataChanged(int priorId, PlayerDataChange changeType, PlayerData postChangeData);

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

        public int PlayerCount => players.Count;

        public int MaxPlayers => maxPlayers;

        public LevelSO SelectedLevel => selectedLevel;

        public event OnPlayerDataChanged PlayerDataChanged;

        [SerializeField]
        [EnableIf("Hide")]
        private readonly List<PlayerData> players = new();

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

            int id = players.Count;
            players.Add(new PlayerData { PlayerId = id });

            PlayerDataChanged?.Invoke(id, PlayerDataChange.PlayerAdded, players[id]);

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

            players.RemoveAt(id);
            for (int i = id; i < players.Count; i++)
            {
                players[i].PlayerId--;
                PlayerDataChanged?.Invoke(i, PlayerDataChange.IdChanged, players[i]);
            }

            PlayerDataChanged?.Invoke(id, PlayerDataChange.PlayerRemoved, null);
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
                PlayerDataChanged?.Invoke(i, PlayerDataChange.PlayerRemoved, players[i]);
            }

            players.Clear();
        }
    }
}