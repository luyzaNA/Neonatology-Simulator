using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {

        public int score = 0;
        public Text scoreText;


        // Use this for initialization
        void Start()
        {
            UpdateUI();
        }

        public void AddPoints(int points)
        {
            score += points;
            UpdateUI();
        }

        public void RemovePoints(int points)
        {
            score -= points;
            if (score < 0)
                score = 0;
            UpdateUI();
        }

        void UpdateUI()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score.ToString();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}