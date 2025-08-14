using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class RaceFinish : MonoBehaviour
{
    public GameObject MyCar;
    public GameObject FinishCam;
    public GameObject ViewModes;
    public GameObject LevelMusic;
    public GameObject CompleteTrig;
    public AudioSource FinishMusic;
    public GameObject KrajDisp;

    void OnTriggerEnter(Collider other)
    {
        Transform root = other.transform.root;
        if (root.CompareTag("Player"))
        {
            if (ModeTime.isTimeMode)
            {
                // logika za time trial mode
            }
            else
            {
                this.GetComponent<BoxCollider>().enabled = false;
                MyCar.SetActive(false);
                CompleteTrig.SetActive(false);
                CarController.m_Topspeed = 0.01f;
                MyCar.GetComponent<CarController>().enabled = false;
                MyCar.GetComponent<CarUserControl>().enabled = false;
                MyCar.SetActive(true);

                FinishCam.SetActive(true);
                LevelMusic.SetActive(false);
                ViewModes.SetActive(false);
                FinishMusic.Play();

                GlobalCash.TotalCash += 100;
                PlayerPrefs.SetInt("SavedCash", GlobalCash.TotalCash);

                KrajDisp.SetActive(true);
            }
        }
    }
}
