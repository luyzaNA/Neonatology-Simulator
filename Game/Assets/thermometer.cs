using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Undercooked
{
    public class thermometer : MonoBehaviour
    {
        public Slider therm;
        public float maxTemp = 45;
        public float temp;
        private Queue<float> temperatureHistory = new Queue<float>();

        // Start is called before the first frame update
        void Start()
        {
            temp = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (therm.value != temp)
            {
                therm.value = temp;
            }

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
                RandomTemp();
            //}
        }

        public void RandomTemp()
        {
            System.Random random = new System.Random();
            float newTemperature = (float)(random.NextDouble() * (40 - 36) + 36);

            temperatureHistory.Enqueue(newTemperature);

            if (temperatureHistory.Count > 100)
            {
                temperatureHistory.Dequeue();
            }

            float averageTemperature = 0;
            foreach (float temp in temperatureHistory)
            {
                averageTemperature += temp;
            }
            averageTemperature /= temperatureHistory.Count;

            temp = averageTemperature;
        }
    }
}
