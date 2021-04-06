using System.Collections;
using System.Collections.Generic;
using Fluffy;
using UnityEngine;

public class LightSwitch : Interactable
{
    public override Vector2 InteractionBubbleOffset { get; }
    
    public override void OnTargeted() { }
    public override void OnUntargeted() { }
    public override void StartInteraction(GameObject interactor) { }
    public override void EndInteraction() { }
}
