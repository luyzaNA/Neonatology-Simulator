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

        public void ShowHint(string message)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayHint(message));
        }

        IEnumerator DisplayHint(string message)
        {
            if ( hintText != null)
            {
                hintText.text = message;
                hintText.enabled = true;
                yield return new WaitForSeconds(hintDUration);
                hintText.enabled = false;
            }
        }
    }
}