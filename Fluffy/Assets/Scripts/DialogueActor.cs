using Fluffy;
using UnityEngine;

public class DialogueActor : MonoBehaviour
{
    public string characterName;
    public Vector2 dialogueBubbleOffset;

    public Vector3 PositionWithOffset =>
        transform.position + new Vector3(dialogueBubbleOffset.x, dialogueBubbleOffset.y);

    private void Start()
    {
        if (CharacterView.instance == null)
        {
            Debug.LogError(
                "Couldn't find the CharacterView instance! Is the 3D Dialogue prefab and CharacterView script in the scene?");
            return;
        }

        CharacterView.instance.RegisterCharacter(this);
    }

    private void OnDestroy()
    {
        CharacterView.instance.ForgetCharacter(this);
    }
}
