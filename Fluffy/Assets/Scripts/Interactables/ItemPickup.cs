using Fluffy;
using UnityEngine;

namespace Fluffy
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private InteractionPromptProvider interactionPromptProvider;
        [SerializeField] private Vector2 interactionBubbleOffset;

        [SerializeField] private string itemName;

        public override Vector2 InteractionBubbleOffset => interactionBubbleOffset;

        public override void OnTargeted()
        {
            interactionPromptProvider.ShowPrompt(this,
                $"Pick up {ItemSpriteMatcher.Instance.GetDisplayName(itemName)}");
        }

        public override void OnUntargeted()
        {
            interactionPromptProvider.HidePrompt(this);
        }

        public override void StartInteraction(GameObject interactor)
        {
            interactor.GetComponent<IInventory>().AddItem(itemName);
            Destroy(gameObject);
        }
    }
}
