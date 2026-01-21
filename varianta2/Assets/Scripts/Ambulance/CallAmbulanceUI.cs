using UnityEngine;

public class CallAmbulanceUI : MonoBehaviour
{
    public AmbulanceController ambulance;

    public GameObject StartGameButton;

    public void CallAmbulance()
    {
        Debug.Log("Buton apăsat!");  
        if (ambulance != null)
            ambulance.StartMoving();
        else
            Debug.Log("Ambulance este null!");

        if (StartGameButton != null)
            StartGameButton.SetActive(false);
    }

}
