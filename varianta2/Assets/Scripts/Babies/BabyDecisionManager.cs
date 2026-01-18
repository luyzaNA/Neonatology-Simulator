using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Managers;

public class BabyDecisionManager : MonoBehaviour
{
    public GameManager gameManager; // referință la GameManager pentru puncte
    public GameObject decisionPanel; // panelul cu decizia
    public TextMeshProUGUI symptomText;
    public TextMeshProUGUI questionText;

    [Header("Hint Text")]
    public TextMeshProUGUI hintText;

    // Exemplu de simptome grave
    private string[] graveSymptoms = new string[]
    {
        "Respirație dificilă",
        "Piele albastră",
        "Bebeluș letargic"
    };

    void Start()
    {
        if (decisionPanel != null)
            decisionPanel.SetActive(false); // ascundem panelul la start
    }

    public void ShowDecisionPanel()
    {
        if (decisionPanel != null)
        {
            decisionPanel.SetActive(true);
        }

        if (symptomText != null)
            symptomText.text = string.Join("\n", graveSymptoms);

        if (questionText != null)
            questionText.text = "Ce urmează să faci cu bebelușul?";
    }

    // Butonul Incubator
    public void ChooseIncubator()
    {
        gameManager.AddPoints(10);
        gameManager.ShowHint("Corect! Bebelușul a fost pus în incubator și primește îngrijire adecvată.");
       // decisionPanel.SetActive(true);
    }


    // Butonul Medicine
    public void ChooseMedicine()
    {
        gameManager.AddPoints(0);
        gameManager.ShowHint("Greșit! Medicina singură nu e suficientă.Bebelușul are nevoie de incubator.");
       // decisionPanel.SetActive(false);
    }

    // Butonul Nothing
    public void ChooseNothing()
    {
        gameManager.AddPoints(0);
        gameManager.ShowHint("Greșit! Nu ai făcut nimic. Bebelușul are nevoie de ajutor urgent!");
       // decisionPanel.SetActive(false);
    }
}
