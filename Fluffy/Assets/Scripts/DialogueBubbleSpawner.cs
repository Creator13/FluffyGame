using UnityEngine;

namespace Fluffy
{
    [RequireComponent(typeof(Canvas))]
    public class DialogueBubbleSpawner : MonoBehaviour
    {
        [SerializeField] private DialogueBubble bubblePrefab;

        private DialogueBubble bubble;
        
        private void Awake()
        {
            bubble = Instantiate(bubblePrefab, transform);
            bubble.gameObject.SetActive(false);
        }

        public void ShowBubble(string text)
        {
            bubble.gameObject.SetActive(true);
            bubble.Text = text;
        }
    }
}
