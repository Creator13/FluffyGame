using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fluffy
{
    public class ItemSpriteMatcher : Singleton<ItemSpriteMatcher>
    {
        [Serializable]
        private struct Item
        {
            public string name;
            public string displayName;
            public Sprite sprite;
        }

        [SerializeField] private List<Item> items;

        private Dictionary<string, Sprite> spritesDictionary;

        private Dictionary<string, Sprite> SpritesDictionary
        {
            get
            {
                if (spritesDictionary == null || spritesDictionary.Count != items.Count)
                {
                    BuildItemDictionaries();
                }

                return spritesDictionary;
            }
        }
        
        private Dictionary<string, string> displayNamesDictionary;

        private Dictionary<string, string> DisplayNamesDictionary
        {
            get
            {
                if (displayNamesDictionary == null || displayNamesDictionary.Count != items.Count)
                {
                    BuildItemDictionaries();
                }

                return displayNamesDictionary;
            }
        }

        private void Awake()
        {
            BuildItemDictionaries();
        }

        public Sprite GetSprite(string itemName)
        {
            return SpritesDictionary[itemName];
        }

        public string GetDisplayName(string itemName)
        {
            return DisplayNamesDictionary[itemName];
        }

        private void BuildItemDictionaries()
        {
            spritesDictionary = new Dictionary<string, Sprite>(items.Count);
            displayNamesDictionary = new Dictionary<string, string>(items.Count);
            
            foreach (var item in items)
            {
                spritesDictionary.Add(item.name, item.sprite);
            }
            
            foreach (var item in items)
            {
                displayNamesDictionary.Add(item.name, item.displayName);
            }
        }
    }
}
