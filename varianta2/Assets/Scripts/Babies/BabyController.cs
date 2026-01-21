using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Managers;
using System.Collections;

public class BabyController : MonoBehaviour
{
    [Header("Symptoms UI")]
    public GameObject decisionPanel;       
    public TextMeshProUGUI symptomsText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI generalMessage;

    [Header("Buttons")]
    public Button incubatorButton;
    public Button giveMedicineButton;
    public Button doNothingButton;

    public bool inIncubator = false;
    public bool inBed = false;
    public bool inAmbulance = false;
    public bool inWaitingRoom = false;
    public bool isHighestPriority = false;
    public GameManager gameManager;
    public bool isFirstBaby = false;

    public BabyDecisionManager babyDecisionManager;

    private string[] severeSymptoms = new string[]
    {
        "Respirație dificilă",
        "Febră mare",
        "Letargie",
        "Paloare severă"
    };

    private string[] severeSymptoms2 = new string[]
    {
        "Vărsături persistente",
        "Convulsii",
        "Deshidratare severă",
        "Febră mare sau instabilă",
        "Letargie severă și refuz alimentar"
    };

    private string[] severeSymptoms3 = new string[]
    {
        "Convulsii atipice sau prelungite",
        "Alterarea stării de conștiență",
        "Semne de presiune intracraniană crescută"
    };

    private string[] moderateSymptoms = new string[]
    {
        "Febră moderată (37.5–38.5°C)",
        "Tuse persistentă",
        "Respirație ușor accelerată"
    };


    private string[] mildSymptoms = new string[]
    {
        "Febră ușoară",
        "Nas înfundat",
        "Strănut frecvent",
        "Somn agitat"
    };



    public void SetInIncubator(bool isInIncubator)
    {
        inIncubator = isInIncubator;
    }

    public void SetInBed(bool isInBed)
    {
        inBed = isInBed;
    }

    private void Start()
    {
        if (decisionPanel != null) decisionPanel.SetActive(false);
    }
    private IEnumerator CloseModal()
    {
        yield return new WaitForSeconds(4f);

        decisionPanel.SetActive(false);
       
    }

    private void OnMouseDown()
    {
        if(inAmbulance)
            ShowDecisionPanelAmbulance();

        if (inWaitingRoom)
            ShowDecisionPanelWaitingRoom();
    }

    public void ShowDecisionPanelTemperature()
    {
        ClearDecisionPanel();
        decisionPanel.SetActive(true);

        string displayText = "Temepratura este foarte ridicata. Au fost regasite si urmatoarele simptome aditionale.\n";
        foreach (var s in severeSymptoms3)
        {
            displayText += "• " + s + "\n";
        }
        symptomsText.text = displayText;

        questionText.text = "Cum tratezi bebelușul?";

        if (incubatorButton != null)
            SetButton(
                incubatorButton,
                "Efectueaza CT",
                () =>
                {
                    gameManager.AddPoints(50);
                    gameManager.ShowHint("Corect! Simptomele indică o posibilă problemă neurologică ce necesită investigații imagistice rapide. Ai câștigat 50 PUNCTE.", Color.green);

                    babyDecisionManager.ChooseCT();
                }
            );

        if (giveMedicineButton != null)
            SetButton(
                giveMedicineButton,
                "Adinistreaza medicamente",
                () =>
                {
                    gameManager.ShowHint(
                        "Corect! Evoluția simptomelor ridică suspiciuni ce impun o investigație rapidă pentru clarificarea cauzei. Nu ai câștigat PUNCTE.",
                        Color.red
                    );
                }
            );

        if (doNothingButton != null)
            SetButton(
                doNothingButton,
                "Trimite-l acasa",
                DoNothing
            );
        

    }

    public void ShowDecisionPanelCT()
    {
        ClearDecisionPanel();
        decisionPanel.SetActive(true);

        string displayText = "CT-ul a indicat o stare de sanatate nu tocmai buna.\n";
        symptomsText.text = displayText;

        questionText.text = "Cum tratezi bebelușul?";

        if (incubatorButton != null)
            SetButton(
                incubatorButton,
                "Interneaza bebelus",
                () =>
                {
                    gameManager.AddPoints(50);
                    gameManager.ShowHint("Corect! Bebelușul are nevoie de internare. Ai castigate 50 PUNCTE.", Color.green);

                    babyDecisionManager.ChooseInternare();
                }
            );

        if (giveMedicineButton != null)
            SetButton(
                giveMedicineButton,
                "Adinistreaza medicamente",
                () =>
                {
                    gameManager.ShowHint(
                        "Greșit! Starea bebelușului necesită supraveghere continuă și reevaluare medicală. Nu ai câștigat PUNCTE.",
                        Color.red
                    );
                }
            );

        if (doNothingButton != null)
            SetButton(
                doNothingButton,
                "Trimite-l acasa",
                DoNothing
            );


    }

    public void ShowDecisionPanelWaitingRoom()
    {
        if (!isHighestPriority)
        {

            ClearDecisionPanel();
            decisionPanel.SetActive(true);
            string[] symptoms = isFirstBaby ? moderateSymptoms : mildSymptoms;

            string displayText = "Simptome prezente:\n";
            foreach (var s in symptoms)
            {
                displayText += "• " + s + "\n";
            }
            symptomsText.text = displayText;

            questionText.text = "Cum tratezi bebelușul?";

            if (incubatorButton != null)
                SetButton(
                    incubatorButton,
                    "Verifica temperatura",
                    () =>
                    {
                        gameManager.ShowHint("Gresit! Un alt bebelus este mai prioritar, cu simptome grave! Nu ai primit PUNCTE.", Color.red);                    }
                );

            if (giveMedicineButton != null)
                SetButton(
                    giveMedicineButton,
                    "Adinistreaza medicamente",
                    () =>
                    {
                        gameManager.ShowHint("Gresit! Un alt bebelus este mai prioritar, cu simptome grave! Nu ai primit PUNCTE.", Color.red);
                    }
                );

            if (doNothingButton != null)
                SetButton(
                    doNothingButton,

                    "Trateaza alt bebelus",
                     () =>
                     {
                         gameManager.AddPoints(20);
                         gameManager.ShowHint("Corect! Un alt bebelus este mai prioritar! Ai castigate 20 PUNCTE.", Color.green);
                         StartCoroutine(CloseModal());
                     }
                );
        }

        if(isHighestPriority)
        {
            ClearDecisionPanel();
            decisionPanel.SetActive(true);

            string displayText = "Simptome prezente:\n";
            foreach (var s in severeSymptoms2)
            {
                displayText += "• " + s + "\n";
            }
            symptomsText.text = displayText;

            questionText.text = "Cum tratezi bebelușul?";

            if (incubatorButton != null)
                SetButton(
                    incubatorButton,
                    "Verifica temperatura",
                    () =>
                    {
                        gameManager.ShowHint("Corect! Bebelușul are nevoie de verificare temperaturii. Ai castigate 20 PUNCTE.", Color.green);
                        gameManager.AddPoints(20);
                        babyDecisionManager.ChooseTemperatureCheck();
                    }
                );

            if (giveMedicineButton != null)
                SetButton(
                    giveMedicineButton,
                    "Adinistreaza medicamente",
                    () =>
                    {
                        gameManager.ShowHint(
                            "Greșit! Starea bebelușului nu a fost evaluată suficient în acest moment. Nu ai câștigat PUNCTE.",
                            Color.red
                        );
                    }
                );

            if (doNothingButton != null)
                SetButton(
                    doNothingButton,
                    "Trimite-l acasa",
                    DoNothing
                );
        }

    }

    private void ClearDecisionPanel()
    {
        if (symptomsText != null)
            symptomsText.text = string.Empty;

        if (questionText != null)
            questionText.text = string.Empty;

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
        ClearDecisionPanel();

        if (decisionPanel == null || symptomsText == null || questionText == null)
            return;

        decisionPanel.SetActive(true);

        symptomsText.text = "Bebelușul este în incubator.";
        questionText.text = "Cum continui să ai grijă de bebeluș?";

        SetButton(
            incubatorButton,
            "Nu face nimic",
            () =>
            {
                gameManager.AddPoints(0);
                gameManager.ShowHint("Greșit! Bebelușul are nevoie de supraveghere continuă.", Color.red);
            }
        );

        SetButton(
            giveMedicineButton,
            "Verifică continuu funcțiile vitale",
            () =>
            {
                gameManager.AddPoints(10);
                gameManager.ShowHint("Corect! Bebelușul este monitorizat constant. Ai obtinut 10 PUNCTE!", Color.green);
                StartCoroutine(HideIncubatorPanelDelayed(3f));
            }
        );

        SetButton(
            doNothingButton,
            "Externează bebelușul",
            () =>
            {
                gameManager.AddPoints(0);
                gameManager.ShowHint("Greșit! Bebelușul nu este stabil pentru externare.", Color.red);
                
            }
        );
    }

    private void ShowDecisionPanelAmbulance()
    {
        if (decisionPanel != null && symptomsText != null && questionText != null)
        {
            decisionPanel.SetActive(true);

            string displayText = "Simptome prezente:\n";
            foreach (var s in severeSymptoms)
            {
                displayText += "• " + s + "\n";
            }
            symptomsText.text = displayText;

            questionText.text = "Cum tratezi bebelușul?";

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

    private IEnumerator HideIncubatorPanelDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Afișăm panelul de îngrijire în incubator
        decisionPanel.SetActive(false);
    }

    private IEnumerator ShowGeneralMessage(string message)
    {   generalMessage.text = message;
        yield return new WaitForSeconds(4f);
        generalMessage.text = "";
    }



    private void GiveMedicine()
    {
        Debug.Log("I s-a dat medicament bebelusului");
        babyDecisionManager.ChooseMedicine();

    }

    private void DoNothing()
    {
        Debug.Log("Nu s-a facut nimic");
        babyDecisionManager.ChooseNothing();

    }
}
