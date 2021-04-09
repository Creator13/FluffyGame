using System;
using System.Collections;
using System.Collections.Generic;
using CameraBounding;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;
using Utility;
using Yarn.Unity;
using Random = UnityEngine.Random;

namespace Fluffy
{
    public class Sequencer : MonoBehaviour
    {
        private enum StartChapter
        {
            Intro,
            Pillowmonster,
            Kitchen,
            Ending
        }

        [SerializeField] private bool skip;
        [SerializeField] private StartChapter startChapter;

        [Header("Global")] [SerializeField] private InteractionController interactionController;
        [SerializeField] private FadeToBlackPanel blackPanel;
        [SerializeField] private DialogueRunner dialogue;
        [SerializeField] private RobinController robin;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Light2D mainLight;
        [SerializeField] private InMemoryVariableStorage variables;
        [SerializeField] private AudioSource music;
        [SerializeField] private AudioSource backgroundFX;
        [SerializeField] private new FollowPlayer2D camera;
        [SerializeField] private InteractionPromptProvider interactionPromptProvider;
        [SerializeField] private FluffyProperties fluffyProperties;

        [Header("Intro")] [SerializeField] private PlayableDirector intro;
        [SerializeField] private LightSwitch bedlight;
        [SerializeField] private PlayableDirector whereIsFluffy;
        [SerializeField] private string startNode = "Intro1";
        [SerializeField] private Transform robinStartSpawn;
        [SerializeField] private GameObject bedCollider;
        [SerializeField] private ItemPickup fluffysEar;
        [SerializeField] private string foundEar = "Intro2";
        [SerializeField] private LightSwitch ceilingLamp;

        [Header("Pillow Monster")] [SerializeField] private List<ItemPickup> pillows;
        [SerializeField] private RoomPortal pillowFortEntrance;
        [SerializeField] private ConversationTarget mayIEnter;
        [SerializeField] private int pillowsNeeded;
        [SerializeField] private ConversationTarget pillowMonster;

        [Header("Kitchen")] [SerializeField] private AutoTrigger kitchenEntranceTrigger;
        [SerializeField] private List<GameObject> hideMonsters;

        [Header("Ending")] [SerializeField] private AutoTrigger bedroomEntranceTrigger;
        [SerializeField] private GameObject tail;
        [SerializeField] private Transform endingStartPosition;
        [SerializeField] private ConversationTarget talkToBedMonster;
        [SerializeField] private GameObject bedMonster;
        [SerializeField] private Transform talkWithBedMonsterPositíon;
        [SerializeField] private PlushyMixNMatch fluffyInHands;

        [Header("Credits")] [SerializeField] private PlayableDirector creditSequence;
        [SerializeField] private AudioClip pianoMusic;
        [SerializeField] private CameraBounds2D polaroidScreen;

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

            bedroomEntranceTrigger.gameObject.SetActive(false);
            bedMonster.gameObject.SetActive(false);
            talkToBedMonster.gameObject.SetActive(false);

            switch (startChapter)
            {
                case StartChapter.Intro:
                    SetIntroState();

                    intro.stopped += OnIntroEnd;
                    intro.Play();

                    break;
                case StartChapter.Pillowmonster:
                    IntroCompleteState();
                    SetPillowmonsterState();

                    break;
                case StartChapter.Kitchen:
                    IntroCompleteState();
                    SetKitchenState();

                    break;
                case StartChapter.Ending:
                    IntroCompleteState();
                    SetEndingState();
                    robin.Move(endingStartPosition.position);
                    
                    // Simulate random fluffy
                    var colors = new [] {"pink", "orange", "green"};
                    var accessories = new [] {"collar", "bowtie", "tshirt"};
                    var patterns = new [] {"stars", "stripes", "hearts"};
                    fluffyProperties.SetFluffyProperty("color", colors[Random.Range(0, 3)]);
                    fluffyProperties.SetFluffyProperty("accessory", accessories[Random.Range(0, 3)]);
                    fluffyProperties.SetFluffyProperty("pattern", patterns[Random.Range(0, 3)]);
                    Debug.Log($"Generated random fluffy {fluffyProperties.Color}-{fluffyProperties.Pattern}-{fluffyProperties.Accessory}");
                    break;
            }
        }


        #region Starting states

        private void SetSkipState()
        {
            blackPanel.SetAmount(1);
            mainLight.intensity = 1;
            bedCollider.SetActive(false);
            robin.Move(robinStartSpawn.position);
        }

        private void SetIntroState()
        {
            mainLight.intensity = .02f;
            blackPanel.FadeOut(0);
            interactionController.Deactivate();
            robin.SetSit(true);
            fluffysEar.gameObject.SetActive(false);
            music.Stop();

            hideMonsters.ForEach(obj => obj.SetActive(false));
            kitchenEntranceTrigger.GetComponent<Collider2D>().enabled = false;

            SetPillowsActive(false);
        }

        private void IntroCompleteState()
        {
            bedlight.StartInteraction(gameObject);
            robin.Move(robinStartSpawn.position);
            bedCollider.SetActive(false);
            mainLight.intensity = .3f;
            if (fluffysEar) fluffysEar.gameObject.SetActive(false);
            ceilingLamp.StartInteraction(gameObject);
        }

        private void SetPillowmonsterState()
        {
            bedCollider.SetActive(false);
            mainLight.intensity = .3f;

            if (!inventory.Has("fluffyEar"))
            {
                inventory.AddItem("fluffyEar");
            }

            pillowFortEntrance.gameObject.SetActive(false);
            SetPillowsActive(false);
            backgroundFX.volume = .2f;
            music.Play();

            hideMonsters.ForEach(obj => obj.SetActive(false));
            kitchenEntranceTrigger.GetComponent<Collider2D>().enabled = false;

            mayIEnter.ConversationStarted += WaitForMayIEnter;
        }

        private void SetKitchenState()
        {
            SetPillowsActive(false);
            hideMonsters.ForEach(obj => obj.SetActive(true));
            kitchenEntranceTrigger.GetComponent<Collider2D>().enabled = true;

            kitchenEntranceTrigger.Targeted += StartKitchenConvo;
        }

        private void SetEndingState()
        {
            bedroomEntranceTrigger.gameObject.SetActive(true);

            bedroomEntranceTrigger.Targeted += ActivateMonsterTail;
        }

        #endregion


        #region Intro

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

            SetPillowmonsterState();
        }

        #endregion


        #region Pillow monster

        private void WaitForMayIEnter()
        {
            mayIEnter.ConversationStarted -= WaitForMayIEnter;

            dialogue.onDialogueComplete.AddListener(CanEnter);
        }

        private void CanEnter()
        {
            dialogue.onDialogueComplete.RemoveListener(CanEnter);

            pillowFortEntrance.gameObject.SetActive(true);

            inventory.Updated += CheckPillowPickup;
        }

        private void CheckPillowPickup()
        {
            // dialogue.variableStorage.SetValue("pillow_count", inventory.ItemCount("pillow"));

            // dialogue.variableStorage.TryGetValue("pillows_needed", out float pillowCount);
            if (inventory.ItemCount("pillow") == pillowsNeeded)
            {
                dialogue.onNodeComplete.AddListener(OnPillowFortComplete);
            }
        }

        private void OnPillowFortComplete(string nodeName)
        {
            if (nodeName != "IHavePillows")
            {
                return;
            }

            dialogue.onNodeComplete.RemoveListener(OnPillowFortComplete);
            pillowMonster.GetComponent<Collider2D>().enabled = false;

            SetKitchenState();
        }

        #endregion


        #region Kitchen

        private void StartKitchenConvo()
        {
            kitchenEntranceTrigger.Targeted -= StartKitchenConvo;

            dialogue.StartDialogue("SlimyBrocFight");
            dialogue.onDialogueComplete.AddListener(AfterFight);

            camera.AdditionalXOffset = 5.4f;
        }

        private void AfterFight()
        {
            dialogue.onDialogueComplete.RemoveListener(AfterFight);

            camera.AdditionalXOffset = 0;

            PrepareFinalScene();
        }

        #endregion


        #region Ending

        private void PrepareFinalScene()
        {
            SetEndingState();
        }

        private void ActivateMonsterTail()
        {
            bedroomEntranceTrigger.Targeted -= ActivateMonsterTail;

            tail.SetActive(true);
            interactionController.Deactivate();
            
            StartCoroutine(Delay(2.5f, WhosThere));
        }

        private void WhosThere()
        {
            dialogue.StartDialogue("WhoIsThere");

            dialogue.onDialogueComplete.AddListener(TalkToBedMonster);
        }

        private void TalkToBedMonster()
        {
            dialogue.onDialogueComplete.RemoveListener(TalkToBedMonster);

            interactionPromptProvider.ShowPrompt(talkToBedMonster, "Check bed", true);
            talkToBedMonster.gameObject.SetActive(true);
            dialogue.onDialogueComplete.AddListener(StartFinalDialogue);
        }

        private void StartFinalDialogue()
        {
            dialogue.onDialogueComplete.RemoveListener(StartFinalDialogue);
            
            blackPanel.FadeOut(.2f);
            StartCoroutine(Delay(.2f, () =>
            {
                robin.Move(talkWithBedMonsterPositíon.position);
                
                blackPanel.FadeIn(.2f);
                StartCoroutine(Delay(.2f, () =>
                {
                    bedMonster.gameObject.SetActive(true);
                    var fluffy = bedMonster.GetComponentInChildren<PlushyMixNMatch>();
                    fluffy.SetBaseColor(fluffyProperties.Color);
                    fluffy.SetAccessory(fluffyProperties.Accessory);
                    fluffy.SetPattern(fluffyProperties.Pattern);
                    dialogue.StartDialogue("MakingFriends");
                    dialogue.onDialogueComplete.AddListener(RollCredits);
                }));
            }));
        }

        private void RollCredits()
        {
            dialogue.onDialogueComplete.RemoveListener(RollCredits);
            interactionController.Deactivate();
            
            creditSequence.Play();
        }

        public void OpenPolaroid()
        {
            music.clip = pianoMusic;
            music.Play();
            camera.SetTarget(polaroidScreen.transform);
            camera.Bounds = polaroidScreen;
        }
        
        #endregion


        private IEnumerator Delay(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }

        private void SetPillowsActive(bool active)
        {
            foreach (var pillow in pillows)
            {
                if (!pillow) continue;
                pillow.GetComponent<Collider2D>().enabled = active;
            }
        }

        public void ActivatePillowSearch()
        {
            SetPillowsActive(true);
        }
    }
}
