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

    [field: SerializeField] private bool LightStatus { get; set; }

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

        UpdateLights();

        if (onlyOnce)
        {
            canInteract = false;
            GetComponent<Collider2D>().enabled = false;
        }
        
        // Update the prompt
        interactionPromptProvider.ShowPrompt(this, $"Light {(LightStatus ? "off" : "on")}");
        
        if (LightStatus)
        {
            LightOn?.Invoke();
        }
        else
        {
            LightOff?.Invoke();
        }
    }

    private void UpdateLights()
    {
        foreach (var light in lights)
        {
            light.enabled = LightStatus;
        }
    }

    private void OnValidate()
    {
        UpdateLights();
    }
}
