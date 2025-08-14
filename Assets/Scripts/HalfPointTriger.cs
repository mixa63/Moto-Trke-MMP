using UnityEngine;

public class HalfPointTrig : MonoBehaviour
{
    public GameObject LapCompleteTrig;  // finish trigger
    private bool playerPassed = false;  // da registruje samo jednom po krugu

    private void OnTriggerEnter(Collider other)
    {
        Transform root = other.transform.root;

        if (root.CompareTag("Player") && !playerPassed)
        {
            playerPassed = true;
            Debug.Log("HalfPoint trigger hit by Player: " + root.name);

            // aktiviraj finish trigger
            if (LapCompleteTrig != null)
                LapCompleteTrig.SetActive(true);

            // deaktiviraj sebe dok se ne resetuje na sledećem krugu
            gameObject.SetActive(false);
        }
    }

    // resetuje trigger za sledeći krug
    public void ResetHalfPoint()
    {
        playerPassed = false;
        gameObject.SetActive(true);

        if (LapCompleteTrig != null)
            LapCompleteTrig.SetActive(false);
    }

    public bool PlayerPassedHalf => playerPassed; // da LapComplete proveri
}
