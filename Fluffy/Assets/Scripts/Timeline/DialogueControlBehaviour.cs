using Fluffy.Dialogue;
using UnityEngine.Playables;

namespace Fluffy.Timeline
{
    public class DialogueControlBehaviour : PlayableBehaviour
    {
        public TimelineDialogueView dialogueView;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (dialogueView != null)
            {
                dialogueView.Next();
            }
        }
    }
}
