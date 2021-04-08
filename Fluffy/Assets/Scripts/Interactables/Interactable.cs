using UnityEngine;
using UnityEngine.Events;

namespace Fluffy
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract Vector2 InteractionBubbleOffset { get; }
        public virtual bool InteractionAvailable { get; set; } = true;
        
        public abstract void OnTargeted();
        public abstract void OnUntargeted();
        public abstract void StartInteraction(GameObject interactor);
    }
}
