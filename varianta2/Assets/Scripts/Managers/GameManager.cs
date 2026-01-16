using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public ScoreManager ScoreManager;
        public HintManager HintManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void GivePoints(int points)
        {
            if(ScoreManager != null)
                ScoreManager.AddPoints(points);
        }

        public void ShowHint(string hint)
        {
            if (HintManager != null)
                HintManager.ShowHint(hint);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}