using System.Collections;
using System.Collections.Generic;
using Fluffy;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

[RequireComponent(typeof(PlayerInput), typeof(PlayerInteraction))]
public class InteractionDialogueView : DialogueViewBase
{
    private PlayerInput input;
    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    public override void DialogueStarted()
    {
        playerInteraction.enabled = false;
        
        input.actions["Walk"].Disable();
        input.actions["Interact"].Disable();
    }

    public override void DialogueComplete()
    {
        playerInteraction.enabled = true;
        
        input.actions["Walk"].Enable();
        input.actions["Interact"].Enable();
    }
}
