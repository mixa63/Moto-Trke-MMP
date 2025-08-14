using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosDown1 : MonoBehaviour
{
   public GameObject positionDisplay;

   void OnTriggerExit(Collider other){

    if(other.tag == "CarPos1"){

        positionDisplay.GetComponent<TMPro.TextMeshProUGUI> ().text = "2nd Place";
    }
   }
}
