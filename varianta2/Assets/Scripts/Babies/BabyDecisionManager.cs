using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Managers;
using Assets.Scripts.Nurse;
using System.Collections;

public class BabyDecisionManager : MonoBehaviour
{
    public GameManager gameManager; // referință la GameManager pentru puncte
    public GameObject decisionPanel; // panelul cu decizia
    public TextMeshProUGUI symptomText;
    public TextMeshProUGUI questionText;
    public NurseController nurseController;

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
    private IEnumerator StopWaypointAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    // Butonul Incubator
    public void ChooseIncubator()
    {
        gameManager.AddPoints(10);
        gameManager.ShowHint(
            "Corect! Bebelușul a fost pus în incubator și primește îngrijire adecvată. Ai primit 10 PUNCTE!", Color.green
        );

        StartCoroutine(ChooseIncubatorSequence());
    }

    public void ChooseTemperatureCheck()
    {
        StartCoroutine(ChooseTemperatureCheckSequence());
    }

    public void ChooseCT()
    {
        StartCoroutine(ChooseCTSequence());
    }
    public void ChooseInternare()
    {
        StartCoroutine(ChooseInternareSequence());
    }

    private IEnumerator ChooseIncubatorSequence()
    {
        yield return new WaitForSeconds(4f);

        decisionPanel.SetActive(false);
        nurseController.TryPickUpBaby();
        nurseController.followWaypoints = true;
    }

    private IEnumerator ChooseTemperatureCheckSequence()
    {
        yield return new WaitForSeconds(4f);

        decisionPanel.SetActive(false);
        nurseController.TryPickUpBaby();
        nurseController.followWaypointsTemperature = true;
    }

    private IEnumerator ChooseCTSequence()
    {
        yield return new WaitForSeconds(4f);

        decisionPanel.SetActive(false);
        nurseController.followWaypointsCT = true;
    }

    private IEnumerator ChooseInternareSequence()
    {
        yield return new WaitForSeconds(4f);

        decisionPanel.SetActive(false);
        nurseController.followWaypointsInternare = true;
    }



    // Butonul Medicine
    public void ChooseMedicine()
    {
        gameManager.AddPoints(0);
        gameManager.ShowHint("Greșit! Medicina singură nu e suficientă.Bebelușul are nevoie de incubator. Nu ai primit PUNCTE!", Color.red);
       // decisionPanel.SetActive(false);
    }

    // Butonul Nothing
    public void ChooseNothing()
    {
        gameManager.AddPoints(0);
        gameManager.ShowHint("Greșit! Nu ai făcut nimic. Bebelușul are nevoie de ajutor urgent! Nu ai primit PUNCTE!", Color.red);
       // decisionPanel.SetActive(false);
    }
}
