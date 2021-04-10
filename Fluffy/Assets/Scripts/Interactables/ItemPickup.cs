using Fluffy;
using UnityEngine;

namespace Fluffy
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private InteractionPromptProvider interactionPromptProvider;
        [SerializeField] private Vector2 interactionBubbleOffset;

        [SerializeField] private string itemName;
        private static readonly int Pick = Animator.StringToHash("Pick");

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
            var anim = interactor.GetComponent<Animator>();
            if (anim)
            {
                anim.SetTrigger(Pick);
            }
            
            interactor.GetComponent<IInventory>().AddItem(itemName);
            Destroy(gameObject);
        }
    }
}
