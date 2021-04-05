using UnityEngine;

namespace Fluffy
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract Vector2 InteractionBubbleOffset { get; }
        
        public abstract void OnTargeted();
        public abstract void OnUntargeted();
        public abstract void StartInteraction(GameObject interactor);
        public abstract void EndInteraction();
    }
}
