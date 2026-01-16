using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Babies
{
    public class BabyUI : MonoBehaviour
    {

        public Baby baby;
        public Slider hungerSlider;
        public Slider comfortSlider;
        public Slider healthSlider;

        // Update is called once per frame
        void Update()
        {
            if (baby != null)
            {
                if (hungerSlider != null)
                    hungerSlider.value = baby.hunger;
                if (comfortSlider != null)
                    comfortSlider.value = baby.confort;
                if (healthSlider != null)
                    healthSlider.value = baby.health;
            }

        }
    }
}