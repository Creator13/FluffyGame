using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fluffy
{
    [System.Serializable]
    public class PlushyOption
    {
        public string name;
        public Sprite sprite;
    }
    
    public class PlushyMixNMatch : MonoBehaviour
    {
        [SerializeField] private List<PlushyOption> baseColors = new List<PlushyOption>(3);
        [SerializeField] private List<PlushyOption> accessories = new List<PlushyOption>(3);
        [SerializeField] private List<PlushyOption> patterns = new List<PlushyOption>(3);

        [SerializeField] private SpriteRenderer baseColorSpriteRenderer;
        [SerializeField] private SpriteRenderer accessorySpriteRenderer;
        [SerializeField] private SpriteRenderer patternSpriteRenderer;

        [Range(0, 2)] public int baseColorIndex;
        [Range(0, 2)] public int accessoryIndex;
        [Range(0, 2)] public int patternIndex;

        public void SetBaseColor(string optionName)
        {
            baseColorIndex = baseColors.FindIndex(opt =>  opt.name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            UpdateSprites();
        }

        public void SetAccessory(string optionName)
        {
            accessoryIndex = accessories.FindIndex(opt => opt.name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            UpdateSprites();
        }

        public void SetPattern(string optionName)
        {
            patternIndex = patterns.FindIndex(opt => opt.name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            UpdateSprites();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateSprites();
        }
#endif

        private void UpdateSprites()
        {
            baseColorSpriteRenderer.sprite = baseColors[baseColorIndex].sprite;
            accessorySpriteRenderer.sprite = accessories[accessoryIndex].sprite;
            patternSpriteRenderer.sprite = patterns[patternIndex].sprite;
        }
    }
}
