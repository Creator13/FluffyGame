using UnityEngine;
using Yarn.Unity;

namespace Fluffy.Dialogue
{
    public class TimelineDialogueView : DialogueViewBase
    {
        public void Next()
        {
            ReadyForNextLine();
        }
    }
}
