using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimeManager : MonoBehaviour
{
    public static int MinuteCount;
    public static int SecondCount;
    public static float MiliCount;
    public static string MiliDisplay;

    public GameObject MinuteBox;
    public GameObject SecondBox;
    public GameObject MiliBox;

    public static float RawTime;


    // Update is called once per frame
    void Update()
    {
        MiliCount += Time.deltaTime * 10;
        RawTime += Time.deltaTime;
        MiliDisplay = MiliCount.ToString("F0");
        MiliBox.GetComponent<TMPro.TextMeshProUGUI> ().text = "" + MiliDisplay;

        if(MiliCount>=10){
            MiliCount = 0;
            SecondCount += 1;
        }
        if(SecondCount<=9){
            SecondBox.GetComponent<TMPro.TextMeshProUGUI> ().text = "0" + SecondCount + ".";
        }else{
            SecondBox.GetComponent<TMPro.TextMeshProUGUI> ().text = "" + SecondCount + ".";
        }

        if(SecondCount>=60){
            SecondCount=0;
            MinuteCount+=1;
        }

        if(MinuteCount<=9){
           MinuteBox.GetComponent<TMPro.TextMeshProUGUI> ().text = "0" + MinuteCount + ":";
        }else{
            MinuteBox.GetComponent<TMPro.TextMeshProUGUI> ().text = "" + MinuteCount + ":";
        }

    }
}
