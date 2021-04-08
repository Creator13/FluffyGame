using UnityEngine;
using Yarn.Unity;

namespace Fluffy.Dialogue
{
    public class SequencerDialogueCommands : MonoBehaviour
    {
        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] private Sequencer sequencer;
        [SerializeField] private Inventory inventory;
        
        private void Awake()
        {
            dialogueRunner.AddCommandHandler("ActivatePillowSearch", ActivatePillowSearch);
            dialogueRunner.AddFunction("pillow_count", PillowCount);
        }

        private void ActivatePillowSearch()
        {
            sequencer.ActivatePillowSearch();
        }

        private int PillowCount()
        {
            return inventory.ItemCount("pillow");
        }
    }
}
