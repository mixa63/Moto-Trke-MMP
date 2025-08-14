using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeScore : MonoBehaviour
{
    public int ModeSelection;
    public GameObject RaceUI;
    public GameObject ScoreUI;
    public GameObject AICar;
    public static int CurrentScore;
    public int InternalScore=0;
    public GameObject ScoreValue;
    public GameObject ScoreObjects;
    public GameObject RaceFinish;

    void Start()
    {
        ModeSelection = ModeSelect.RaceMode;
        if(ModeSelection==1){
            RaceUI.SetActive(false);
            ScoreUI.SetActive(true);
            AICar.SetActive(false);
            ScoreObjects.SetActive(true);
        }
    }
    

    void Update(){
        InternalScore = CurrentScore;
         ScoreValue.GetComponent<TMPro.TextMeshProUGUI> ().text = "" + InternalScore;
        if (InternalScore>=500){
            RaceFinish.SetActive(true);
            StartCoroutine(ToMenu());
        }
    }
    

     IEnumerator ToMenu(){
        
        yield return new WaitForSeconds(3);
        ButtonOption b = new ButtonOption();
        b.TrackSelect();
    }
    
}
