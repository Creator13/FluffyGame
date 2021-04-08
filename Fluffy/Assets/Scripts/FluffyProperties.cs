using UnityEngine;
using Yarn.Unity;

public class FluffyProperties : MonoBehaviour
{
    public string Color { get; private set; }
    public string Accessory  { get; private set; }
    public string Pattern { get; private set; }

    [SerializeField] private DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler<string, string>("SetFluffyProperty", SetFluffyProperty);
    }

    private void SetFluffyProperty(string property, string value) 
    {
        switch (property)
        {
            case "color":
                Color = value;
                break;
            case "accessory":
                Accessory = value;
                break;
            case "pattern":
                Pattern = value;
                break;
            default:
                Debug.LogError($"Illegal property {property}.");
                break;
        }
    }
}
