using Input_Scripts;
using UnityEngine;

/// <summary>
///     Temporary character select UI component
///     Spawns PlayerReadyController when a controller is conencted
/// </summary>
public class PlayerReady : MonoBehaviour
{
    [SerializeField]
    private GameObject playerReady;

    [SerializeField]
    private PlayerInputSo playerInputSO;

    private void OnEnable()
    {
        playerInputSO.ClearAllInputReaders();
        playerInputSO.PlayerInputAdded += OnInputAdded;
    }

    private void OnDisable()
    {
        playerInputSO.PlayerInputAdded -= OnInputAdded;
    }

    private void OnInputAdded(int id)
    {
        Debug.Log($"Player {id} connected");
        var button = Instantiate(playerReady, transform).GetComponent<PlayerReadyController>();
        button.Initialize(id);
    }
}