using UnityEngine;
using Yarn.Unity;

namespace Fluffy
{
    [RequireComponent(typeof(InteractionController))]
    public class InteractionDialogueView : DialogueViewBase
    {
        private InteractionController interactionController;

        private void Awake()
        {
            interactionController = GetComponent<InteractionController>();
        }

        public override void DialogueStarted()
        {
            interactionController.Deactivate();
        }

        public override void DialogueComplete()
        {
            interactionController.Activate();
        }
    }
}
