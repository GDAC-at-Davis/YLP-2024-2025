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

        for (var i = 0; i < gameDataSO.PlayerCount; i++)
        {
            var character =
                Instantiate(gameDataSO.GetPlayerData(i).SelectedCharacter.CharacterPrefab, spawns[i],
                    Quaternion.identity).GetComponent<CharacterEntity>();
            character.Initialize(i);
            players.Add(character);
            players[i].UpdateHealth += CheckHealth;
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

        players[id].UpdateHealth -= CheckHealth;
        players[id].gameObject.SetActive(false);

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