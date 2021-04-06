using Fluffy.Dialogue;
using UnityEngine.Playables;

namespace Fluffy.Timeline
{
    public class DialogueControlBehaviour : PlayableBehaviour
    {
        public TimelineDialogueView dialogueView;

        public override void OnGraphStart(Playable playable)
        {
            if (dialogueView != null)
            {
                dialogueView.ReadyForNextLine();
            }
        }
    }
}
