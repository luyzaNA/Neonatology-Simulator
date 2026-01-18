using Assets.Scripts.Managers;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Babies
{
    public class Baby : MonoBehaviour
    {
        public string babyName;
        public BabyPersonality personality;

        public bool inCrib = false;

        public float hunger = 50;
        public float confort = 80;
        public float health = 100;

        public BabyState state;

        // Update is called once per frame
        void Update()
        {
            hunger += personality.hungerRate * Time.deltaTime;
            confort -= personality.comfortDecay * Time.deltaTime;
            hunger = Mathf.Clamp(hunger, 0, 100);
            confort = Mathf.Clamp(confort, 0, 100);

            EvaluateState();
        }

        void EvaluateState()
        {
            if (hunger > personality.cryTolerance || confort < 30)
                state = BabyState.Crying;
            else if (health < 50)
                state = BabyState.Sick;
            else
                state = BabyState.Happy;
        }

        public void Feed()
        {
            hunger -= 40;
            confort += 10;
            state = BabyState.Happy;
            //GameManager.Instance.GivePoints(10);
            GameManager.Instance.ShowHint($"{babyName} has been fed and is now happy!");
        }

        public void ChangeDiaper()
        {
            confort += 30;
            state = BabyState.Happy;
            //GameManager.Instance.GivePoints(5);
            GameManager.Instance.ShowHint($"{babyName}'s diaper has been changed and is now comfortable!");
        }

        public void CheckTemperature()
        {
            if (health < 70)
                GameManager.Instance.ShowHint($"{babyName} feels cold. Consider warming them up.");
        }

        public void SetInCrib(bool isInCrib)
        {
            inCrib = isInCrib;
        }

        // Additional methods for playing, putting to sleep, and medical care can be added here
    }
}