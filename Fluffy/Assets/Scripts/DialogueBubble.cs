using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Fluffy
{
    public class DialogueBubble : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public string Text
        {
            set => text.text = value;
        }
    }
}
