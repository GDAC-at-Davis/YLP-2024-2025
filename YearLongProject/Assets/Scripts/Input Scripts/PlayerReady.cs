using Input_Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Temporary character select UI component
///  Spawns playerreadybutton when a controller is conencted
/// </summary>
public class PlayerReady : MonoBehaviour
{
    [SerializeField]
    GameObject playerReady;
    [SerializeField]
    PlayerInputSo playerInputSO;

    private void OnEnable()
    {
        playerInputSO.PlayerInputAdded += OnInputAdded;
    }

    private void OnDisable()
    {
        playerInputSO.PlayerInputAdded -= OnInputAdded;
    }

    void OnInputAdded(int id)
    {
        PlayerReadyButton button = Instantiate(playerReady, transform).GetComponent<PlayerReadyButton>();
        button.Initialize(id);
    }
}
