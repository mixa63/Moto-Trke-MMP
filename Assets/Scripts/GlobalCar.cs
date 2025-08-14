using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCar : MonoBehaviour
{
    public static int CarType;  // Prvi 1 , Drugi 2
    public GameObject TrackWindow;
    

    public void RedCar(){
        CarType = 1;
        TrackWindow.SetActive(true);
    }
    
    public void WhiteCar(){
        CarType = 2;
        TrackWindow.SetActive(true);
    }
    public void GreyCar(){
        CarType = 3;
        TrackWindow.SetActive(true);
    }
}
