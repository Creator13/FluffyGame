using UnityEngine;
using Yarn.Unity;

namespace Fluffy
{
    public class ConversationTarget : Interactable
    {
        [SerializeField] private string promptText;
        [SerializeField] private InteractionPromptProvider interactionPromptProvider;

        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] private string dialogueStartNode;
        [SerializeField] private Vector2 interactionBubbleOffset;

        public override Vector2 InteractionBubbleOffset => interactionBubbleOffset;

        public override void OnTargeted()
        {
            interactionPromptProvider.ShowPrompt(this, promptText);
        }

        public override void OnUntargeted()
        {
            interactionPromptProvider.HidePrompt(this);
        }

        public override void StartInteraction(GameObject interactor)
        {
            interactionPromptProvider.HidePrompt(this);

            dialogueRunner.StartDialogue(dialogueStartNode);
        }

        public override void EndInteraction() { }
    }
}
