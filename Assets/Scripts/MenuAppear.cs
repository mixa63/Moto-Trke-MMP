using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAppear : MonoBehaviour
{
    public GameObject largeButton;
    public GameObject textClick;
    public GameObject menuButton;

    public void StartMenu(){
        textClick.SetActive(false);
        menuButton.SetActive(true);
        largeButton.SetActive(false);
    }
}
