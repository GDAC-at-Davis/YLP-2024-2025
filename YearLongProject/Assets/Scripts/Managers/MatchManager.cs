using System.Collections.Generic;
using System.Linq;
using GameEntities;
using Input_Scripts;
using Managers;
using UnityEngine;

/// <summary>
///     Instantiates stage and characters
/// </summary>
public class MatchManager : MonoBehaviour
{
    [SerializeField]
    private GameDataSO gameDataSO;

    [SerializeField]
    private PlayerInputSo playerInputSO;

    private readonly List<CharacterEntity> players = new();

    private void Awake()
    {
        Vector3[] spawns = Instantiate(gameDataSO.SelectedLevel.LevelPrefab).Spawnpoints;

        if (spawns.Length < gameDataSO.PlayerCount)
        {
            Debug.LogError($"There should be at least {gameDataSO.PlayerCount} spawnpoints!");
        }

        foreach (int id in gameDataSO.GetPlayerIds())
        {
            var character = Instantiate(
                gameDataSO.GetPlayerData(id).SelectedCharacter.CharacterPrefab,
                spawns[players.Count],
                Quaternion.identity).GetComponent<CharacterEntity>();

            character.Initialize(id);
            players.Add(character);
            players[^1].UpdateHealth += CheckHealth;
        }
    }

    private void OnDisable()
    {
        foreach (CharacterEntity player in players)
        {
            player.UpdateHealth -= CheckHealth;
        }
    }

    private void CheckHealth(int id, int health)
    {
        if (health > 0)
        {
            return;
        }

        int playerEntityIndex = players.FindIndex(item => item.PlayerId == id);

        players[playerEntityIndex].UpdateHealth -= CheckHealth;
        players[playerEntityIndex].gameObject.SetActive(false);

        if (players.Where(item => item.gameObject.activeInHierarchy).Count() > 1)
        {
            return;
        }

        Invoke("NewGame", 3);
    }

    private void NewGame()
    {
        gameDataSO.LoadScene("FighterSelect");
    }
}