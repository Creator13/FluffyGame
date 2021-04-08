using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Fluffy
{
    public class InteractionPrompt : MonoBehaviour
    {
        [SerializeField] private TMP_Text textObject;
        [SerializeField] private GameObject buttonHint;

        public void Show(string text, bool showButtonHint = true)
        {
            gameObject.SetActive(false);
            textObject.text = text;
            buttonHint.SetActive(showButtonHint);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
