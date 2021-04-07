using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    public Sprite Image
    {
        set => image.sprite = value;
    }

    public string Text
    {
        set => text.text = value;
    }
}
