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
    
    [Header("Intro")] [SerializeField] private PlayableDirector intro;
    [SerializeField] private LightSwitch bedlight;
    
    private void Awake()
    {
        Assert.IsNotNull(interactionController);
        Assert.IsNotNull(blackPanel);
        Assert.IsNotNull(intro);
        Assert.IsNotNull(bedlight);
        
        SetInitialState();
        
        intro.stopped += OnIntroEnd;
        intro.Play();
    }

    private void SetInitialState()
    {
        blackPanel.FadeOut(0);
        interactionController.Deactivate();
    }
    
    private void OnIntroEnd(PlayableDirector director)
    {
        intro.gameObject.SetActive(false);
        
        bedlight.GetComponent<Collider2D>().enabled = true;
        interactionController.Activate();
        bedlight.LightOn += OnLightOn;
    }

    private void OnLightOn()
    {
        blackPanel.FadeIn(0);
    }

    private IEnumerable Delay(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }
}
