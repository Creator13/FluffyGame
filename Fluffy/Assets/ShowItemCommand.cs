using System.Collections;
using Fluffy;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(BubblePlacer))]
public class ShowItemCommand : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private ItemDisplay itemDisplay;
    [SerializeField] private DialogueActor playerDialogueActor;

    private BubblePlacer bubblePlacer;
    
    public void Awake()
    {
        bubblePlacer = GetComponent<BubblePlacer>();
        
        itemDisplay.gameObject.SetActive(false);

        dialogueRunner.AddCommandHandler<string, float>("ShowItem", ShowItemCommandHandler);
    }

    private Coroutine ShowItemCommandHandler(string itemName, float time = 0.5f)
    {
        var image = ItemSpriteMatcher.Instance.GetSprite(itemName);
        var text = ItemSpriteMatcher.Instance.GetDisplayName(itemName);

        return StartCoroutine(DoShowItem(image, text, time));
    }

    private IEnumerator DoShowItem(Sprite image, string text, float time)
    {
        itemDisplay.Image = image;
        itemDisplay.Text = text;
        
        itemDisplay.gameObject.SetActive(true);

        if (time > 0)
        {
            yield return new WaitForSeconds(time);
        }
        
        itemDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (itemDisplay.gameObject.activeInHierarchy)
        {
            bubblePlacer.PlaceBubble((RectTransform) itemDisplay.transform, playerDialogueActor.transform.position + (Vector3) playerDialogueActor.dialogueBubbleOffset);
        }
    }
}
