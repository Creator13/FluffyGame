using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class FadeToBlackPanel : MonoBehaviour
    {
        [SerializeField] private Image background;
        [Range(0, 1)] [SerializeField] private float amount;

        public void FadeIn(float time)
        {
            if (time <= 0)
            {
                SetAmount(1);
                return;
            }

            StartCoroutine(DoFadeIn(time));
        }

        public void FadeOut(float time)
        {
            if (time <= 0)
            {
                SetAmount(0);
                return;
            }

            StartCoroutine(DoFadeOut(time));
        }

        private void Update()
        {
            SetFadeAmount();
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            SetFadeAmount();
        }

#endif

        public void SetAmount(float amount)
        {
            this.amount = amount;
        }
        
        private void SetFadeAmount()
        {
            var newColor = background.color;
            newColor.a = 1 - amount;
            background.color = newColor;
        }

        private IEnumerator DoFadeIn(float time)
        {
            while (amount < 1)
            {
                amount += Time.deltaTime / time;
                amount = Mathf.Clamp(amount, 0, 1);
                yield return null;
            }
        }

        private IEnumerator DoFadeOut(float time)
        {
            while (amount > 0)
            {
                amount -= Time.deltaTime / time;
                amount = Mathf.Clamp(amount, 0, 1);
                yield return null;
            }
        }
    }
}
