using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarControlActive : MonoBehaviour
{
    public GameObject CarControl;
    public GameObject CarControl2;
    // Start is called before the first frame update
    void Start()
    {
        CarControl.GetComponent<CarUserControl>().enabled = true;
        CarControl2.GetComponent<CarAIControl>().enabled = true;
    }

    
}
