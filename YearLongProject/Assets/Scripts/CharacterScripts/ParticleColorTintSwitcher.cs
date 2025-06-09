using EditorUtils.BoldHeader;
using GameEntities;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterScripts
{
    public class ParticleColorTintSwitcher : MonoBehaviour
    {
        [BoldHeader("Material Switcher")]
        [InfoBox("Switches color based on player color")]
        [SerializeField]
        private ParticleSystem particleSystem;

        [SerializeField]
        private CharacterEntity characterEntity;

        private void Start()
        {
            var ps = particleSystem.main;
            ps.startColor = characterEntity.PlayerColor;
        }
    }
}