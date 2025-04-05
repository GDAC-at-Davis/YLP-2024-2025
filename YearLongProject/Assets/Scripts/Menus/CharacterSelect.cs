using System.Collections.Generic;
using CharacterScripts;
using GameEntities;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Menus
{
    /// <summary>
    ///     Temporary Character Select system
    ///     When all players are ready, load game scene and characters
    ///     Opting for singleton rather than SO here since we'll only need this in the characterselect scene
    /// </summary>
    public class CharacterSelect : MonoBehaviour
    {
        public static CharacterSelect Instance;

        [SerializeField]
        [Scene]
        private string gameSceneName;

        public UnityAction AllPlayersReady;

        [SerializeField]
        private readonly Dictionary<int, CharacterSO> playerReady = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += GameStarted;
            }

            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= GameStarted;
        }

        public void ReadyUp(int id, CharacterSO character)
        {
            if (!playerReady.TryGetValue(id, out _))
            {
                playerReady.Add(id, character);
            }

            playerReady[id] = character;
            TryStartGame();
        }

        public void TryStartGame()
        {
            if (playerReady.ContainsValue(null))
            {
                return;
            }

            AllPlayersReady.Invoke();
            Invoke("StartGame", 1);
        }

        private void StartGame()
        {
            // TODO move this logic to an SO
            SceneManager.LoadScene(gameSceneName);
        }

        private void GameStarted(Scene scene, LoadSceneMode sceneMode)
        {
            foreach (int id in playerReady.Keys)
            {
                Instantiate(playerReady[id].CharacterPrefab).GetComponent<CharacterEntity>()
                    .Initialize(id);
            }
        }
    }
}