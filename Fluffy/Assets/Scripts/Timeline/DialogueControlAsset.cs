using Fluffy.Dialogue;
using UnityEngine;
using UnityEngine.Playables;

namespace Fluffy.Timeline
{
    public class DialogueControlAsset : PlayableAsset
    {
        public ExposedReference<TimelineDialogueView> dialogue;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return new ScriptPlayable<DialogueControlBehaviour>();
        }
    }
}
