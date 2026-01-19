using UnityEngine;
using System.Collections;

public class AmbulanceController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public Transform startPoint;
    public Transform stopPoint;

    [Header("Exit Settings")]
    public GameObject stretcher;
    public GameObject doctor;
    public Transform exitPoint;

    [Header("Audio")]
    public AudioSource sirenAudio;

    private bool moving = false;

    void Start()
    {
        // La start, ambulanța începe la startPoint
        if (startPoint != null)
            transform.position = startPoint.position;

        // Doctorul și targa sunt invizibile la început
        if (doctor != null)
            doctor.SetActive(false);

        if (stretcher != null)
            stretcher.SetActive(false);

        moving = false;
    }

    public void StartMoving()
    {
        moving = true;

        // Asigură-te că ambulanța pornește de la start
        if (startPoint != null)
            transform.position = startPoint.position;

        // Pornim sirena doar dacă AudioSource-ul e activ
        if (sirenAudio != null && sirenAudio.enabled)
            sirenAudio.Play();
    }

    void Update()
    {
        if (!moving) return;

        // Muta ambulanța spre stop
        transform.position = Vector3.MoveTowards(
            transform.position,
            stopPoint.position,
            speed * Time.deltaTime
        );

        // Când ajunge la stop, oprește mișcarea și sirena
        if (Vector3.Distance(transform.position, stopPoint.position) < 0.1f)
        {
            moving = false;

            if (sirenAudio != null && sirenAudio.isPlaying)
                sirenAudio.Stop();

            StartCoroutine(ExitAmbulanceSequence());
        }
    }

    IEnumerator ExitAmbulanceSequence()
    {
        // Mic delay înainte să iasă doctorul
        yield return new WaitForSeconds(1f);

        if (stretcher != null && exitPoint != null)
        {
            // Scoatem targa la exitPoint
            stretcher.transform.position = exitPoint.position;
            stretcher.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        if (doctor != null && exitPoint != null)
        {
            // Scoatem doctorul cu un mic offset față de targa
            Vector3 doctorOffset = new Vector3(-0.7f, 1f, 0f); // 1.5 unități pe X
            doctor.transform.position = exitPoint.position + doctorOffset;
            doctor.SetActive(true);
        }
    }

}
