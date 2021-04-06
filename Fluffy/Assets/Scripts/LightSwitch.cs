using System;
using System.Collections.Generic;
using Fluffy;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightSwitch : Interactable
{
    public event Action LightOn; 
    public event Action LightOff; 
    
    [SerializeField] private List<Light2D> lights;
    [SerializeField] private bool onlyOnce;
    [SerializeField] private InteractionPromptProvider interactionPromptProvider;

    private bool canInteract = true;

    private bool LightStatus { get; set; } = false;

    public override Vector2 InteractionBubbleOffset { get; } = Vector2.zero;
    public override bool InteractionAvailable => canInteract;

    public override void OnTargeted()
    {
        interactionPromptProvider.ShowPrompt(this, $"Light {(LightStatus ? "off" : "on")}");
    }

    public override void OnUntargeted()
    {
        interactionPromptProvider.HidePrompt(this);
    }

    public override void StartInteraction(GameObject interactor)
    {
        LightStatus = !LightStatus;

        foreach (var light in lights)
        {
            light.enabled = LightStatus;
        }
        
        if (onlyOnce) canInteract = false;
        
        if (LightStatus)
        {
            LightOn?.Invoke();
        }
        else
        {
            LightOff?.Invoke();
        }
    }
    public override void EndInteraction() { }
}
