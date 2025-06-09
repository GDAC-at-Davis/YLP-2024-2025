using UnityEngine;

namespace CharacterScripts
{
    public class Gravestone : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        
        public void Initialize(Color c)
        {
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer is not assigned.");
                return;
            }

            spriteRenderer.color = c;
        }
    }
}
