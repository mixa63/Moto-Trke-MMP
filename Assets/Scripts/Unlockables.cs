using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unlockables : MonoBehaviour
{
    public GameObject greenButton;
    public int cashValue;
    // Update is called once per frame
    void Update()
    {
        cashValue = GlobalCash.TotalCash;
        if(cashValue>=150){
            greenButton.GetComponent<Button>().interactable = true;
        }
    }
    public void GreenUnlock(){
        greenButton.SetActive(false);
        cashValue -= 150;
    }
}
