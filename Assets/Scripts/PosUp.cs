using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    
   public GameObject positionDisplay;

   void onTriggerExit(Collider other){

    if(other.tag== "CarPos"){

        positionDisplay.GetComponent<Text>().text="1st Place";
    }
   }
}
