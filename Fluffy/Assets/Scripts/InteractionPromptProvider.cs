using UnityEngine;
using UnityEngine.Serialization;

namespace Fluffy
{
    [RequireComponent(typeof(BubblePlacer))]
    public class InteractionPromptProvider : MonoBehaviour
    {
        [FormerlySerializedAs("interactionBubble")] [SerializeField] private InteractionPrompt interactionPrompt;

        [SerializeField] private float bubbleMargin = 0.1f;

        private Interactable target;

        private BubblePlacer bubblePlacer;

        private void Awake()
        {
            bubblePlacer = GetComponent<BubblePlacer>();
            interactionPrompt.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (target)
            {
                bubblePlacer.PlaceBubble(interactionPrompt.transform as RectTransform,
                    target.transform.position + (Vector3) target.InteractionBubbleOffset, bubbleMargin);
            }
        }

        public void ShowPrompt(Interactable target, string text)
        {
            this.target = target;
            interactionPrompt.Show(text);
        }

        /// <summary>
        /// By passing the original requester of a prompt, we can avoid objects hiding another object's prompt.
        /// </summary>
        /// <param name="target">The target which requested the prompt it wants to hide.</param>
        public void HidePrompt(Interactable target)
        {
            if (target == this.target)
            {
                interactionPrompt.Hide();
                this.target = null;
            }
        }
    }
}
