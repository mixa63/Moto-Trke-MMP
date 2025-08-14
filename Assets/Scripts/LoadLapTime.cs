using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLapTime : MonoBehaviour
{
    public int MinCount;
    public int SecCount;
    public float MiliCount;
    public GameObject MinDislpay;
    public GameObject SecDislpay;
    public GameObject MiliDislpay;
   
    void Start()
    {
        MinCount=PlayerPrefs.GetInt("MinSave");
        SecCount=PlayerPrefs.GetInt("SecSave");
        MiliCount=PlayerPrefs.GetFloat("MiliSave");

        if(SecCount<=9){
             SecDislpay.GetComponent<TMPro.TextMeshProUGUI> ().text = "0" + SecCount + ".";
        }else{
            SecDislpay.GetComponent<TMPro.TextMeshProUGUI> ().text = "" + SecCount + ".";
        }

        if(MinCount<=9){
             MinDislpay.GetComponent<TMPro.TextMeshProUGUI> ().text = "0" + MinCount + ":";
        }else{
           MinDislpay.GetComponent<TMPro.TextMeshProUGUI> ().text = "" + MinCount + ":";
        }

        MiliDislpay.GetComponent<TMPro.TextMeshProUGUI>().text="" + MiliCount;

    }

    
}
