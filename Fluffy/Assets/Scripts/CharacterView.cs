/*
The MIT License (MIT)

Copyright (c) 2015-2021 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

----

Modified by Casper van Battum (copyright 2021).

*/

using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Fluffy
{
    /// <summary>Manager singleton that repositions DialogueUI window in 3D worldspace, based on whoever is speaking. Put this script on the same gameObject as your DialogueUI.</summary>
    [RequireComponent(typeof(BubblePlacer))]
    public class CharacterView : DialogueViewBase
    {
        // very minimal implementation of singleton manager (initialized lazily in Awake)
        public static CharacterView instance;

        // list of all YarnCharacters in the scene, who register themselves in YarnCharacter.Start()
        public List<DialogueActor> allCharacters = new List<DialogueActor>();

        [Tooltip("display dialogue choices for this character, and display any no-name dialogue here too")]
        public DialogueActor playerCharacter;

        private DialogueActor speakerCharacter;

        [Tooltip(
            "for best results, set the rectTransform anchors to middle-center, and make sure the rectTransform's pivot Y is set to 0")]
        public RectTransform dialogueBubbleRect, optionsBubbleRect;

        [Tooltip(
            "margin is 0-1.0 (0.1 means 10% of screen space)... -1 lets dialogue bubbles appear offscreen or get cutoff")]
        public float bubbleMargin = 0.1f;

        private BubblePlacer bubblePlacer;

        // Awake is called before the first frame update AND before Start...
        private void Awake()
        {
            // ... this is important because we must set the static "instance" here, before any YarnCharacter.Start() can use it
            instance = this;
            bubblePlacer = GetComponent<BubblePlacer>();
        }

        /// <summary>automatically called by YarnCharacter.Start() so that YarnCharacterView knows they exist</summary>
        public void RegisterCharacter(DialogueActor newCharacter)
        {
            if (!instance.allCharacters.Contains(newCharacter))
            {
                allCharacters.Add(newCharacter);
            }
        }

        /// <summary>automatically called by YarnCharacter.OnDestroy() to clean-up</summary>
        public void ForgetCharacter(DialogueActor deletedCharacter)
        {
            if (instance.allCharacters.Contains(deletedCharacter))
            {
                allCharacters.Remove(deletedCharacter);
            }
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // Try and get the character name from the line
            var hasCharacterName = dialogueLine.Text.TryGetAttributeWithName("character", out var characterAttribute);

            if (hasCharacterName)
            {
                speakerCharacter = FindCharacter(characterAttribute.Properties["name"].StringValue);
            }
            else
            {
                speakerCharacter = null; // if null, Update() will use the playerCharacter instead
            }

            // IMPORTANT: we must mark this view as having finished its work, or else the DialogueRunner gets stuck forever
            onDialogueLineFinished();
        }

        /// <summary>simple search through allCharacters list for a matching name, returns null and LogWarning if no match found</summary>
        private DialogueActor FindCharacter(string searchName)
        {
            foreach (var character in allCharacters)
            {
                if (character.characterName == searchName)
                {
                    return character;
                }
            }

            Debug.LogWarningFormat("YarnCharacterView couldn't find a YarnCharacter named {0}!", searchName);
            return null;
        }

        private void Update()
        {
            // this all in Update instead of RunLine because characters might walk around or move during the dialogue
            if (dialogueBubbleRect.gameObject.activeInHierarchy)
            {
                if (speakerCharacter != null)
                {
                    bubblePlacer.PlaceBubble(dialogueBubbleRect, speakerCharacter.PositionWithOffset, bubbleMargin);
                }
                else
                {
                    // if no speaker defined, then display speech above playerCharacter as a default
                    bubblePlacer.PlaceBubble(dialogueBubbleRect, playerCharacter.PositionWithOffset, bubbleMargin);
                }
            }

            // put choice option UI above playerCharacter
            if (optionsBubbleRect.gameObject.activeInHierarchy)
            {
                bubblePlacer.PlaceBubble(optionsBubbleRect, playerCharacter.PositionWithOffset, bubbleMargin);
            }
        }


        // these overrides are required when we inherit from DialogueViewBase
        // but if your custom dialogue view doesn't need them, it's ok to leave them empty and unused like this
        public override void DismissLine(Action onDismissalComplete)
        {
            onDismissalComplete();
        }
    }
}
