using System;
using System.Collections;
using Fluffy;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using Utility;
using Yarn.Unity;

public class Sequencer : MonoBehaviour
{
    [Header("Global")] [SerializeField] private InteractionController interactionController;
    [SerializeField] private FadeToBlackPanel blackPanel;
    [SerializeField] private DialogueRunner dialogue;
    [SerializeField] private RobinController robin;
    
    [Header("Intro")] [SerializeField] private PlayableDirector intro;
    [SerializeField] private LightSwitch bedlight;
    [SerializeField] private PlayableDirector whereIsFluffy;
    [SerializeField] private string startNode = "Intro1";
    [SerializeField] private Transform robinStartSpawn;
    
    private void Awake()
    {
        Assert.IsNotNull(interactionController);
        Assert.IsNotNull(blackPanel);
        Assert.IsNotNull(intro);
        Assert.IsNotNull(bedlight);
        Assert.IsNotNull(whereIsFluffy);

        SetInitialState();
        
        intro.stopped += OnIntroEnd;
        intro.Play();
    }

    private void SetInitialState()
    {
        blackPanel.FadeOut(0);
        interactionController.Deactivate();
        robin.SetSit(true);
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
    }

    private IEnumerable Delay(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }
}
