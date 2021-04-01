using System.Security.Cryptography;
using UnityEngine;

namespace Fluffy
{
    public class ConversationTarget : MonoBehaviour, IInteractable
    {
        [SerializeField] private string promptText;
        [SerializeField] private DialogueBubble promptPrefab;
        [SerializeField] private Transform dialogueBubbleAnchor;

        private DialogueBubble prompt;
        
        public void OnTargeted()
        {
            if (prompt)
            {
                Destroy(prompt.gameObject);
            }
            
            prompt = Instantiate(promptPrefab, dialogueBubbleAnchor, false);
            prompt.Text = promptText;
        }

        public void OnUntargeted()
        {
            if (prompt)
            {
                Destroy(prompt.gameObject);
            }
        }

        public void StartInteraction() { }

        public void EndInteraction() { }
    }
}
