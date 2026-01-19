using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class HintManager : MonoBehaviour
    {
        public TextMeshProUGUI hintText;
        public float hintDUration = 4f;

        public void ShowHint(string message, Color? color = null)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayHint(message, color));
        }

        IEnumerator DisplayHint(string message, Color? color = null)
        {
            if ( hintText != null)
            {

                if (color.HasValue)
                    hintText.color = color.Value;

                hintText.text = message;
                hintText.enabled = true;
                yield return new WaitForSeconds(hintDUration);
                hintText.enabled = false;
            }
        }
    }
}