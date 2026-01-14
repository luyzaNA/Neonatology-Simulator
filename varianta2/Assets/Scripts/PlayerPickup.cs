using UnityEngine;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup")]
    public Transform holdPoint;
    public float pickupRange = 1.5f;

    [Header("UI")]
    public TextMeshProUGUI messageText;

    private Baby currentBaby;

    void Start()
    {
        if (messageText != null)
            messageText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentBaby == null)
                TryPickUp();
            else
                TryPlaceBaby();
        }
    }

    // ================= PICK UP =================
    void TryPickUp()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider hit in hits)
        {
            Baby baby = hit.GetComponent<Baby>();
            if (baby != null && !baby.isCarried)
            {
                currentBaby = baby;
                baby.isCarried = true;

                Rigidbody rb = baby.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                baby.transform.SetParent(holdPoint);
                baby.transform.localPosition = Vector3.zero;
                baby.transform.localRotation = Quaternion.identity;

                ShowMessage("Ai luat bebelușul");
                return;
            }
        }

        ShowMessage("Nu este niciun bebeluș aici");
    }

    // ================= PLACE =================
    void TryPlaceBaby()
    {
        if (currentBaby == null)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider hit in hits)
        {
            BabyIncubator incubator = hit.GetComponent<BabyIncubator>();
            if (incubator != null && !incubator.hasBaby && incubator.placePoint != null)
            {
                PlaceBabyOnIncubator(incubator);
                ShowMessage("Bebeluș pus în pat");
                return;
            }
        }

        ShowMessage("Nu poți lăsa bebelușul aici!");
    }

    // ================= FINAL PLACE =================
    void PlaceBabyOnIncubator(BabyIncubator incubator)
    {
        Rigidbody rb = currentBaby.GetComponent<Rigidbody>();

        currentBaby.transform.SetParent(null);
        currentBaby.transform.position = incubator.placePoint.position;
        currentBaby.transform.rotation = incubator.placePoint.rotation;

        if (rb != null)
            rb.isKinematic = true;

        currentBaby.isCarried = false;
        incubator.hasBaby = true;
        currentBaby = null;
    }

    // ================= UI =================
    void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;
    }
}
