using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class scriptaca : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var carController = GetComponent<CarController>();
        if (carController == null)
        {
            Debug.LogError("❌ CarController skripta NIJE pronađena na ovom objektu!");
            return;
        }

        // Rigidbody check
        var rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("❌ Nema Rigidbody na objektu sa CarController-om!");
        else
            Debug.Log("✅ Rigidbody postoji.");

        // WheelCollider check
        var wheelCollidersField = typeof(CarController).GetField("m_WheelColliders",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var wheelColliders = (WheelCollider[])wheelCollidersField.GetValue(carController);
        if (wheelColliders == null || wheelColliders.Length == 0)
            Debug.LogError("❌ Nema ni jedan WheelCollider u CarController-u!");
        else
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                if (wheelColliders[i] == null)
                    Debug.LogError($"❌ WheelCollider na indexu {i} nije postavljen!");
                else
                    Debug.Log($"✅ WheelCollider {i} postoji: {wheelColliders[i].name}");
            }
        }

        // Mesh check
        var wheelMeshesField = typeof(CarController).GetField("m_WheelMeshes",
    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var wheelMeshes = (GameObject[])wheelMeshesField.GetValue(carController);
        if (wheelMeshes == null || wheelMeshes.Length == 0)
            Debug.LogWarning("⚠️ Nema wheel mesh-ova postavljenih (ovo neće baciti grešku ali točkovi se neće okretati).");
        else
        {
            for (int i = 0; i < wheelMeshes.Length; i++)
            {
                if (wheelMeshes[i] == null)
                    Debug.LogWarning($"⚠️ Wheel mesh na indexu {i} nije postavljen!");
                else
                    Debug.Log($"✅ Wheel mesh {i} postoji: {wheelMeshes[i].name}");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
