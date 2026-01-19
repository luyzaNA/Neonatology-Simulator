using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Managers;
using System.Collections;

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

    public bool inIncubator = false;
    public GameManager gameManager;


    public BabyDecisionManager babyDecisionManager;

    // Lista de simptome grave
    private string[] severeSymptoms = new string[]
    {
        "Respirație dificilă",
        "Febră mare",
        "Letargie",
        "Paloare severă"
    };

    public void SetInIncubator(bool isInIncubator)
    {
        inIncubator = isInIncubator;
    }

    private void Start()
    {
        // Setăm panelul și butoanele inactive la start
        if (decisionPanel != null) decisionPanel.SetActive(false);
    }

    private void OnMouseDown()
    {
        if(!inIncubator)
            ShowDecisionPanelAmbulance();
    }

    private void ClearDecisionPanel()
    {
        // Reset text
        if (symptomsText != null)
            symptomsText.text = string.Empty;

        if (questionText != null)
            questionText.text = string.Empty;

        // Reset butoane
        ClearButton(incubatorButton);
        ClearButton(giveMedicineButton);
        ClearButton(doNothingButton);
    }
    private void ClearButton(Button button)
    {
        if (button == null) return;

        button.onClick.RemoveAllListeners();

        TextMeshProUGUI txt = button.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null)
            txt.text = string.Empty;

        button.interactable = true;
    }


    private void SetButton(Button button, string text, UnityEngine.Events.UnityAction action)
    {
        if (button == null) return;

        button.onClick.RemoveAllListeners();

        TextMeshProUGUI txt = button.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null)
            txt.text = text;

        button.onClick.AddListener(action);
    }


    public void ShowDecisionPanelIncubator()
    {
        // 🔥 curățăm tot înainte
        ClearDecisionPanel();

        if (decisionPanel == null || symptomsText == null || questionText == null)
            return;

        decisionPanel.SetActive(true);

        symptomsText.text = "Bebelușul este în incubator.";
        questionText.text = "Cum continui să ai grijă de bebeluș?";

        // ❌ OPȚIUNE GREȘITĂ
        SetButton(
            incubatorButton,
            "Nu face nimic",
            () =>
            {
                gameManager.AddPoints(0);
                gameManager.ShowHint("Greșit! Bebelușul are nevoie de supraveghere continuă.");
            }
        );

        // ✅ OPȚIUNE CORECTĂ
        SetButton(
            giveMedicineButton,
            "Verifică continuu funcțiile vitale",
            () =>
            {
                gameManager.AddPoints(10);
                gameManager.ShowHint("Corect! Bebelușul este monitorizat constant.");
                StartCoroutine(HideIncubatorPanelDelayed(3f));
            }
        );

        // ❌ OPȚIUNE GREȘITĂ
        SetButton(
            doNothingButton,
            "Externează bebelușul",
            () =>
            {
                gameManager.AddPoints(0);
                gameManager.ShowHint("Greșit! Bebelușul nu este stabil pentru externare.");
                
            }
        );
    }



    private void ShowDecisionPanelAmbulance()
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
        babyDecisionManager.ChooseIncubator();
    }

    private IEnumerator ShowIncubatorPanelDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Afișăm panelul de îngrijire în incubator
        ShowDecisionPanelIncubator();
    }

    private IEnumerator HideIncubatorPanelDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Afișăm panelul de îngrijire în incubator
        decisionPanel.SetActive(false);
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
