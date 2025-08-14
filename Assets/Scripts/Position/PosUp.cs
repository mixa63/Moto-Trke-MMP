using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosUp : MonoBehaviour
{
   public GameObject positionDisplay;

   void OnTriggerExit(Collider other){

    if(other.tag == "CarPos1"){

        positionDisplay.GetComponent <TMPro.TextMeshProUGUI> ().text= "1st Place";
    }
   }
}
