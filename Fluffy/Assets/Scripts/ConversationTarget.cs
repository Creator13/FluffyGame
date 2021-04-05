using System.Security.Cryptography;
using UnityEngine;
using Yarn.Unity;

namespace Fluffy
{
    public class ConversationTarget : MonoBehaviour, IInteractable
    {
        [SerializeField] private string promptText;

        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] private string dialogueStartNode;

        private DialogueBubble prompt;
        
        public void OnTargeted()
        {
            if (prompt)
            {
                Destroy(prompt.gameObject);
            }
            
            // prompt = Instantiate(promptPrefab, dialogueBubbleAnchor, false);
            // prompt.Text = promptText;
        }

        public void OnUntargeted()
        {
            if (prompt)
            {
                Destroy(prompt.gameObject);
            }
        }

        public void StartInteraction(GameObject interactor)
        {
            if (!dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.StartDialogue(dialogueStartNode);
            }
        }

        public void EndInteraction() { }
    }
}
