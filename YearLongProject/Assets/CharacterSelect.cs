using GameEntities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// Temporary Character Select system
/// When all players are ready, load game scene and characters
/// 
/// Opting for singleton rather than SO here since we'll only need this in the characterselect scene
/// </summary>
public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect Instance;

    /// <summary>
    /// Character to spawn
    /// 
    /// When more characters are added will need to implement feature to spawn different characters
    /// </summary>
    [SerializeField]
    GameObject character;

    public UnityAction AllPlayersReady;

    [SerializeField]
    Dictionary<int, bool> playerReady = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += GameStarted;
        }

        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= GameStarted;
    }

    public void ReadyUp(int id, bool ready)
    {
        if (!playerReady.TryGetValue(id, out _))
        {
            playerReady.Add(id, ready);
        }

        playerReady[id] = ready;
        TryStartGame();
    }

    public void TryStartGame()
    {
        if (playerReady.ContainsValue(false)) return;

        AllPlayersReady.Invoke();
        Invoke("StartGame", 1);
    }

    void StartGame()
    {
        // TODO move this logic to an SO
        SceneManager.LoadScene("PrototypeA");
    }

    void GameStarted(Scene scene, LoadSceneMode sceneMode)
    {
        foreach (int id in playerReady.Keys)
        {
            Instantiate(character, Vector3.zero, Quaternion.identity).GetComponent<CharacterEntity>().Initialize(id);
        }
    }
    
    
}
