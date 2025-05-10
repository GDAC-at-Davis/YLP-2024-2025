using GameEntities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterScripts
{
    public class CharacterWorldspaceUI : MonoBehaviour
    {
        [SerializeField]
        private CharacterEntity entity;

        [SerializeField]
        private GameDataSO gameDataSO;

        [SerializeField]
        private Image normalMarker;

        [SerializeField]
        private Image dashMarker;

        private void Start()
        {
            normalMarker.color = gameDataSO.PlayerColors[entity.PlayerId];
            dashMarker.color = gameDataSO.PlayerColors[entity.PlayerId];
            UpdateInvincibilityStatus(false);
        }

        private void OnEnable()
        {
            entity.InvincibleChanged += UpdateInvincibilityStatus;
        }

        private void OnDisable()
        {
            entity.InvincibleChanged -= UpdateInvincibilityStatus;
        }

        private void UpdateInvincibilityStatus(bool isInvuln)
        {
            normalMarker.enabled = !isInvuln;
            dashMarker.enabled = isInvuln;
        }
    }
}