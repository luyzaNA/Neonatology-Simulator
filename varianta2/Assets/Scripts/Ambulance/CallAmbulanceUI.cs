using UnityEngine;

public class CallAmbulanceUI : MonoBehaviour
{
    public AmbulanceController ambulance;

    public void CallAmbulance()
    {
        Debug.Log("Buton apăsat!");  // verificare
        if (ambulance != null)
            ambulance.StartMoving();
        else
            Debug.Log("Ambulance este null!");
    }

}
