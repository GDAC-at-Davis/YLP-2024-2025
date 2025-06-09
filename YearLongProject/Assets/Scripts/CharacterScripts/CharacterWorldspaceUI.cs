using GameEntities;
using UnityEngine;

namespace CharacterScripts
{
    public class CharacterWorldspaceUI : MonoBehaviour
    {
        [SerializeField]
        private CharacterEntity entity;

        [SerializeField]
        private SpriteRenderer normalMarker;

        [SerializeField]
        private SpriteRenderer dashMarker;

        private void Start()
        {
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