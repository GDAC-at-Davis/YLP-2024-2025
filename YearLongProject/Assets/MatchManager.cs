using GameEntities;
using Input_Scripts;
using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Instantiates stage and characters
/// </summary>
public class MatchManager : MonoBehaviour
{
    [SerializeField]
    private GameDataSO gameDataSO;
    [SerializeField]
    private PlayerInputSo playerInputSO;

    private List<CharacterEntity> players = new();

    private void Awake()
    {
        Instantiate(gameDataSO.SelectedLevel.LevelPrefab);
        for (int i = 0; i < gameDataSO.PlayerCount; i++)
        {
            CharacterEntity character = Instantiate(gameDataSO.GetPlayerData(i).SelectedCharacter.CharacterPrefab).GetComponent<CharacterEntity>();
            character.Initialize(i);
            players.Add(character);
            players[i].UpdateHealth += CheckHealth;
        }
    }

    private void OnDisable()
    {
        foreach(CharacterEntity player in players)
        {
            player.UpdateHealth -= CheckHealth;
        }
    }

    void CheckHealth(int id, int health)
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

        foreach(CharacterEntity entity in players)
        {
            gameDataSO.RemovePlayer(entity.PlayerId);
        }
        playerInputSO.ClearAllInputReaders();
        gameDataSO.LoadScene("FighterSelect");
    }
}
