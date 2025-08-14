using System.Collections;
using UnityEngine;
using TMPro;

public class LapComplete : MonoBehaviour
{
    public HalfPointTrig halfPointTrig; // referenca na HalfPointTrig
    public GameObject LapCounter;
    public int LapsDone;
    public int TotalLaps = 3;

    public GameObject MinuteDisplay;
    public GameObject SecondDisplay;
    public GameObject MiliDisplay;
    public GameObject RaceFinishTrig;

    void OnTriggerEnter(Collider other)
    {
        Transform root = other.transform.root;

        if (root.CompareTag("Player"))
        {
            if (halfPointTrig != null && halfPointTrig.PlayerPassedHalf)
            {
                LapsDone++;
                Debug.Log("Lap completed! Total laps: " + LapsDone);

                // update UI
                if (LapCounter != null)
                    LapCounter.GetComponent<TextMeshProUGUI>().text = LapsDone.ToString();

                // reset HalfPointTrig za sledeći krug
                halfPointTrig.ResetHalfPoint();

                // resetuj vreme kruga ako koristiš LapTimeManager
                float RawTime = PlayerPrefs.GetFloat("RawTime");
                if (LapTimeManager.RawTime <= RawTime)
                {
                    SecondDisplay.GetComponent<TextMeshProUGUI>().text =
                        (LapTimeManager.SecondCount <= 9 ? "0" : "") + LapTimeManager.SecondCount + ".";
                    MinuteDisplay.GetComponent<TextMeshProUGUI>().text =
                        (LapTimeManager.MinuteCount <= 9 ? "0" : "") + LapTimeManager.MinuteCount + ":";
                    MiliDisplay.GetComponent<TextMeshProUGUI>().text = LapTimeManager.MiliCount.ToString();
                }

                PlayerPrefs.SetInt("MinSave", LapTimeManager.MinuteCount);
                PlayerPrefs.SetInt("SecSave", LapTimeManager.SecondCount);
                PlayerPrefs.SetFloat("MiliSave", LapTimeManager.MiliCount);
                PlayerPrefs.SetFloat("RawTime", LapTimeManager.RawTime);

                LapTimeManager.MinuteCount = 0;
                LapTimeManager.SecondCount = 0;
                LapTimeManager.MiliCount = 0;
                LapTimeManager.RawTime = 0;

                if (LapsDone >= TotalLaps)
                {

                    Debug.Log("Race Finished!");
                    RaceFinishTrig.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Player nije prošao HalfPoint, laps se ne povećava");
            }
        }
    }
}
