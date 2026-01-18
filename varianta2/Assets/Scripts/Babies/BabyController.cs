using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BabyController : MonoBehaviour
{
    [Header("Symptoms UI")]
    public GameObject decisionPanel;       // Panel cu simptome + intrebare + optiuni
    public TextMeshProUGUI symptomsText;
    public TextMeshProUGUI questionText;

    [Header("Buttons")]
    public Button incubatorButton;
    public Button giveMedicineButton;
    public Button doNothingButton;

    public BabyDecisionManager babyDecisionManager;

    // Lista de simptome grave
    private string[] severeSymptoms = new string[]
    {
        "Respirație dificilă",
        "Febră mare",
        "Letargie",
        "Paloare severă"
    };

    private void Start()
    {
        // Setăm panelul și butoanele inactive la start
        if (decisionPanel != null) decisionPanel.SetActive(false);
    }

    private void OnMouseDown()
    {
        ShowDecisionPanel();
    }

    private void ShowDecisionPanel()
    {
        if (decisionPanel != null && symptomsText != null && questionText != null)
        {
            decisionPanel.SetActive(true);

            // Afisam simptomele
            string displayText = "Simptome prezente:\n";
            foreach (var s in severeSymptoms)
            {
                displayText += "• " + s + "\n";
            }
            symptomsText.text = displayText;

            // Afisam intrebarea
            questionText.text = "Cum tratezi bebelușul?";

            // Setam actiuni pentru butoane
            if (incubatorButton != null)
                incubatorButton.onClick.AddListener(PutInIncubator);

            if (giveMedicineButton != null)
                giveMedicineButton.onClick.AddListener(GiveMedicine);

            if (doNothingButton != null)
                doNothingButton.onClick.AddListener(DoNothing);
        }
    }

    private void PutInIncubator()
    {
        Debug.Log("Bebelusul a fost pus in incubator!");
        babyDecisionManager.ChooseIncubator();
       // ClosePanel();
        // Aici poti sa faci animatie / logica incubator
    }

    private void GiveMedicine()
    {
        Debug.Log("I s-a dat medicament bebelusului");
        babyDecisionManager.ChooseMedicine();

        // ClosePanel();
    }

    private void DoNothing()
    {
        Debug.Log("Nu s-a facut nimic");
        babyDecisionManager.ChooseNothing();

        //ClosePanel();
    }

    private void ClosePanel()
    {
        if (decisionPanel != null)
            decisionPanel.SetActive(false);
    }
}
