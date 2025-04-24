using System.Collections.Generic;
using CharacterScripts;
using EditorUtils.BoldHeader;
using GameEntities;
using Input_Scripts;
using LevelScripts;
using Managers;
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

        [BoldHeader("Character Select")]
        [InfoBox("Handles character selection screen logic")]
        [Header("Depends")]

        [SerializeField]
        private GameDataSO gameDataSO;

        [SerializeField]
        private PlayerInputSo playerInputSO;

        [SerializeField]
        private GameObject characterSelectScreen;

        [SerializeField]
        private GameObject levelSelectScreen;

        [Header("Config")]

        [SerializeField]
        [Scene]
        private string gameSceneName;

        public UnityAction<bool> AllPlayersReady;

        private readonly Dictionary<int, CharacterSO> playerReady = new();

        private LevelSO levelToLoad;

        private void Start()
        {
            // Reset SO data on start
            playerInputSO.ClearAllInputReaders();
            gameDataSO.ClearPlayerData();

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
            TryLevelScreen();
        }

        public void TryLevelScreen()
        {
            if (playerReady.ContainsValue(null))
            {
                return;
            }

            characterSelectScreen.SetActive(false);
            levelSelectScreen.SetActive(true);
            AllPlayersReady.Invoke(true);
        }

        public void ReturnToCharacterSelect()
        {
            levelSelectScreen.SetActive(false);
            characterSelectScreen.SetActive(true);

            for (var i = 0; i < playerReady.Count; i++)
            {
                playerReady[i] = null;
            }

            AllPlayersReady.Invoke(false);
        }

        public void TryStartGame(LevelSO level)
        {
            levelToLoad = level;
            Invoke("StartGame", 1);
        }

        private void StartGame()
        {
            // TODO move this logic to an SO
            SceneManager.LoadScene(gameSceneName);
        }

        private void GameStarted(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name != gameSceneName)
            {
                return;
            }

            Instantiate(levelToLoad.LevelPrefab);
            foreach (int id in playerReady.Keys)
            {
                Instantiate(playerReady[id].CharacterPrefab).GetComponent<CharacterEntity>()
                    .Initialize(id);
            }
        }
    }
}