using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChoice : MonoBehaviour
{
    public GameObject RedBody;
    public GameObject WhiteBody;
    public GameObject GreyBody;
    public int CarImport;
    void Start()
    {
        CarImport = GlobalCar.CarType;
        if(CarImport==1){
            RedBody.SetActive(true);
            GreyBody.SetActive(false);
             WhiteBody.SetActive(false);
        }

        if(CarImport==2){
            RedBody.SetActive(false);
            GreyBody.SetActive(false);
            WhiteBody.SetActive(true);
        }

        if(CarImport==3){
            RedBody.SetActive(false);
            WhiteBody.SetActive(false);
            GreyBody.SetActive(true);
        }
    }

    
}
