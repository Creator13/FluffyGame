using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Fluffy
{
    [System.Serializable]
    public class LazyPlushyOption
    {
        public string name;
        public SpriteRenderer sprite;
    }

    public class LazyPlushyMixNMatch : MonoBehaviour
    {
        [SerializeField] private List<LazyPlushyOption> baseColors = new List<LazyPlushyOption>(3);
        [SerializeField] private List<LazyPlushyOption> accessories = new List<LazyPlushyOption>(3);
        [SerializeField] private List<LazyPlushyOption> patterns = new List<LazyPlushyOption>(3);

        [Range(0, 2)] public int baseColorIndex;
        [Range(0, 2)] public int accessoryIndex;
        [Range(0, 2)] public int patternIndex;

        public void SetBaseColor(string optionName)
        {
            baseColorIndex =
                baseColors.FindIndex(opt => opt.name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            UpdateSprites();
        }

        public void SetAccessory(string optionName)
        {
            accessoryIndex =
                accessories.FindIndex(opt => opt.name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
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
            _ = baseColors.Select((opt, i) =>
            {
                opt.sprite.gameObject.SetActive(i == baseColorIndex);
                return 0;
            });
            _ = accessories.Select((opt, i) =>
            {
                opt.sprite.gameObject.SetActive(i == accessoryIndex);
                return 0;
            });
            _ = patterns.Select((opt, i) =>
            {
                opt.sprite.gameObject.SetActive(i == patternIndex);
                return 0;
            });
        }
    }
}
