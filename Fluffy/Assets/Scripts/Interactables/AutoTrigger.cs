﻿using System;
using UnityEngine;

namespace Fluffy
{
    public class AutoTrigger : Interactable
    {
        public event Action Targeted;
        
        public override Vector2 InteractionBubbleOffset { get; }

        public override void OnTargeted()
        {
            Targeted?.Invoke();
            InteractionAvailable = false;
        }

        public override void OnUntargeted()
        {
            gameObject.SetActive(false);
        }
        public override void StartInteraction(GameObject interactor) { }
    }
}
