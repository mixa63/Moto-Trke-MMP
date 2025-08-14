using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonOption : MonoBehaviour
{
   //public void PlayGame (){
   // SceneManager.LoadScene(2);
   //}
   public void TrackSelect(){
      
    SceneManager.LoadScene(2);
   }
   public void MainMenu(){
    SceneManager.LoadScene(1);
   }

   public void Track01 (){
    SceneManager.LoadScene(3);
   }

   public void Track02 (){
    SceneManager.LoadScene(4);
   }

   public void Credits (){
    SceneManager.LoadScene(5);
   }
   public void doExitGame() {
     Application.Quit();
 }


}

