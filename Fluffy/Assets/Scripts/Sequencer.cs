using System;
using System.Collections;
using System.Collections.Generic;
using Fluffy;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;
using Utility;
using Yarn.Unity;

public class Sequencer : MonoBehaviour
{
    [SerializeField] private bool skip;

    [Header("Global")] [SerializeField] private InteractionController interactionController;
    [SerializeField] private FadeToBlackPanel blackPanel;
    [SerializeField] private DialogueRunner dialogue;
    [SerializeField] private RobinController robin;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Light2D mainLight;

    [Header("Intro")] [SerializeField] private PlayableDirector intro;
    [SerializeField] private LightSwitch bedlight;
    [SerializeField] private PlayableDirector whereIsFluffy;
    [SerializeField] private string startNode = "Intro1";
    [SerializeField] private Transform robinStartSpawn;
    [SerializeField] private GameObject bedCollider;
    [SerializeField] private ItemPickup fluffysEar;
    [SerializeField] private string foundEar = "Intro2";

    [Header("Pillow Monster")] [SerializeField] private List<ItemPickup> pillows;

    private void Awake()
    {
        Assert.IsNotNull(interactionController);
        Assert.IsNotNull(blackPanel);
        Assert.IsNotNull(dialogue);
        Assert.IsNotNull(robin);
        Assert.IsNotNull(inventory);
        Assert.IsNotNull(mainLight);

        Assert.IsNotNull(intro);
        Assert.IsNotNull(bedlight);
        Assert.IsNotNull(whereIsFluffy);
        Assert.IsNotNull(robinStartSpawn);
        Assert.IsNotNull(bedCollider);
        Assert.IsNotNull(fluffysEar);

        if (skip)
        {
            SetSkipState();
            return;
        }

        SetInitialState();

        intro.stopped += OnIntroEnd;
        intro.Play();
    }

    private void SetSkipState()
    {
        blackPanel.SetAmount(1);
        mainLight.intensity = 1;
        bedCollider.SetActive(false);
        robin.Move(robinStartSpawn.position);
    }

    private void SetInitialState()
    {
        blackPanel.FadeOut(0);
        interactionController.Deactivate();
        robin.SetSit(true);
        fluffysEar.gameObject.SetActive(false);

        SetPillowsActive(false);
    }

    private void OnIntroEnd(PlayableDirector _)
    {
        intro.stopped -= OnIntroEnd;

        intro.gameObject.SetActive(false);

        bedlight.GetComponent<Collider2D>().enabled = true;
        interactionController.Activate();
        bedlight.LightOn += OnLightOn;
    }

    private void OnLightOn()
    {
        bedlight.LightOn -= OnLightOn;

        blackPanel.FadeIn(0);
        dialogue.StartDialogue(startNode);
        whereIsFluffy.Play();

        whereIsFluffy.stopped += FluffyIsLost;
    }

    private void FluffyIsLost(PlayableDirector _)
    {
        whereIsFluffy.stopped -= FluffyIsLost;

        robin.SetSit(false);
        robin.Move(robinStartSpawn.position);
        bedCollider.SetActive(false);
        fluffysEar.gameObject.SetActive(true);

        inventory.Updated += CheckEarPickup;
    }

    private void CheckEarPickup()
    {
        if (inventory.Has("fluffyEar"))
        {
            inventory.Updated -= CheckEarPickup;

            dialogue.StartDialogue(foundEar);
            dialogue.onDialogueComplete.AddListener(EndIntro);
        }
    }

    private void EndIntro()
    {
        dialogue.onDialogueComplete.RemoveListener(EndIntro);

        mainLight.intensity = .3f;
    }


    private IEnumerable Delay(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }

    private void SetPillowsActive(bool active)
    {
        foreach (var pillow in pillows)
        {
            pillow.GetComponent<Collider2D>().enabled = active;
        }
    }
}
