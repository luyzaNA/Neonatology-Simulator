using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public ScoreManager ScoreManager;
        public HintManager HintManager;

        [Header("Greeting Settings")]
        public TMP_InputField enterName;
        public GameObject buttonOK;
        public TextMeshProUGUI greeting;
        public GameObject greetingPanel;

        [Header("Player Name UI")]
        public TextMeshProUGUI PlayerNameText;
        public GameObject playerNamePanel;
        public TextMeshProUGUI playerScoreText;

        [Header("Start game")]
        public GameObject Start_game;

        private int score = 0;

        public string PlayerName { get; private set; }


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
        void SetPlayerName(string name)
        {
            PlayerName = name;

            if (PlayerNameText != null)
                PlayerNameText.text = PlayerName.ToUpper();

            if (playerScoreText != null)
                playerScoreText.text = $"Score: {score}";
        }

        public void GivePoints(int points)
        {
            if (ScoreManager != null)
                ScoreManager.AddPoints(points);
        }

        public void ShowHint(string hint)
        {
            if (HintManager != null)
                HintManager.ShowHint(hint);
        }

        void LoadPlayerName()
        {
            if (PlayerPrefs.HasKey("SavedName"))
            {
                PlayerName = PlayerPrefs.GetString("SavedName");
                Debug.Log(PlayerName);
            }

            else
                PlayerName = "Player";

        }

        // Use this for initialization
        void Start()
        {
            if (PlayerPrefs.HasKey("SavedName"))
            {
               // PlayerPrefs.DeleteKey("SavedName");
                SetPlayerName(PlayerPrefs.GetString("SavedName"));

                if (playerNamePanel != null)
                    playerNamePanel.SetActive(false);

                Start_game.SetActive(false);

                ShowGreetingOnly();
                DisplayGreeting(PlayerName);

                StartCoroutine(HideGreetingAfterDelay(5f));

                SetPlayerName(PlayerPrefs.GetString("SavedName"));

                ShowGreetingOnly();
                DisplayGreeting(PlayerName);

                StartCoroutine(HideGreetingAfterDelay(5f));
            }
            else
            {
                ShowNameInputOnly();

                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                enterName.Select();
                enterName.ActivateInputField();
            }
        }


        private void DisplayGreeting(string name)
        {
            if (greeting != null)
            {
                int hour = DateTime.Now.Hour;
                string message;

                if (hour >= 5 && hour < 12)
                    message = "Buna diminneata";
                else if (hour >= 12 && hour < 18)
                    message = "Buna ziua";
                else
                    message = "Buna seara ";

                greeting.text = $"{message}, {name}!";
            }
        }

        public void ConfirmName()
        {
            if (string.IsNullOrWhiteSpace(enterName.text))
                return;

            SetPlayerName(enterName.text);

            PlayerPrefs.SetString("SavedName", PlayerName);
            PlayerPrefs.Save();

            DisplayGreeting(PlayerName);
            ShowGreetingOnly();

            Time.timeScale = 1;

            StartCoroutine(HideGreetingAfterDelay(5f));
        }


        IEnumerator HideGreetingAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            if (greetingPanel != null)
                greetingPanel.SetActive(false);

            if (playerNamePanel != null)
                playerNamePanel.SetActive(true);

            Start_game.SetActive(true);

        }


        void ShowNameInputOnly()
        {
            enterName.gameObject.SetActive(true);
            buttonOK.SetActive(true);
            greeting.gameObject.SetActive(false);
        }

        void ShowGreetingOnly()
        {
            enterName.gameObject.SetActive(false);
            buttonOK.SetActive(false);
            greeting.gameObject.SetActive(true);

            if (playerNamePanel != null)
                playerNamePanel.SetActive(false);
        }


        // Update is called once per frame
        void Update()
        {

        }

        public void AddPoints(int points)
        {
            score += points;
            ScoreManager.setPoints(points);
            UpdatePlayerUI();
        }

        private void UpdatePlayerUI()
        {
            if (PlayerNameText != null)
                PlayerNameText.text = PlayerName.ToUpper();

            if (playerScoreText != null)
                playerScoreText.text = $"Score: {score}";

        }
    }
}
