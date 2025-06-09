using EditorUtils.BoldHeader;
using GameEntities;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterScripts
{
    public class ColorTintSwitcher : MonoBehaviour
    {
        [BoldHeader("Material Switcher")]
        [InfoBox("Switches color based on player color")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private CharacterEntity characterEntity;

        private void Start()
        {
            spriteRenderer.color = characterEntity.PlayerColor;
        }
    }
}