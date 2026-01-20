using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Managers;
using Assets.Scripts.Nurse;
using System.Collections;

public class BabyDecisionManager : MonoBehaviour
{
    public GameManager gameManager; 
    public GameObject decisionPanel;
    public TextMeshProUGUI symptomText;
    public TextMeshProUGUI questionText;
    public NurseController nurseController;

    [Header("Hint Text")]
    public TextMeshProUGUI hintText;

    private string[] graveSymptoms = new string[]
    {
        "Respirație dificilă",
        "Piele albastră",
        "Bebeluș letargic"
    };

    void Start()
    {
        if (decisionPanel != null)
            decisionPanel.SetActive(false); 
    }
    private IEnumerator StopWaypointAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void ChooseIncubator()
    {
        gameManager.AddPoints(10);
        gameManager.ShowHint(
            "Corect! Bebelușul a fost pus în incubator și primește îngrijire adecvată. Ai primit 20 PUNCTE!", Color.green
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



    public void ChooseMedicine()
    {
        gameManager.AddPoints(0);
        gameManager.ShowHint(
            "Greșit! Starea bebelușului necesită un mediu controlat și suport continuu, nu doar tratament medicamentos. Nu ai primit PUNCTE!",
            Color.red
        );
    }

    public void ChooseNothing()
    {
        gameManager.AddPoints(0);
        gameManager.ShowHint("Greșit! Bebelușul are nevoie de ajutor urgent! Nu ai primit PUNCTE!", Color.red);
    }
}
