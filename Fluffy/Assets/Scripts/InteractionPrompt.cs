using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fluffy
{
    public class InteractionPrompt : MonoBehaviour
    {
        [SerializeField] private TMP_Text textObject;

        public void Show(string text)
        {
            gameObject.SetActive(false);
            textObject.text = text;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
