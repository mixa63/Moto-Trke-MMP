using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public GameObject CountDown;
    public AudioSource GetReady;
    public AudioSource LetsGo;
    public GameObject LapTimer;
    public GameObject CarControls;
    public AudioSource LevelMusic;


    // makakjakakh
    void Start()
    {
        LapTimer.SetActive(false);
        StartCoroutine(CountStart());
    }

    IEnumerator CountStart(){
        yield return new WaitForSeconds(0.5f);
        CountDown.GetComponent<TMPro.TextMeshProUGUI> ().text = "3";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<TMPro.TextMeshProUGUI> ().text = "2";
        
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<TMPro.TextMeshProUGUI> ().text = "1";
        
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        LetsGo.Play();
        LevelMusic.Play();
        LapTimer.SetActive(true);
        CarControls.SetActive(true);

    }
}
