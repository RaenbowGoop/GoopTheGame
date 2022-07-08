using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; //add to use scene manager
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
   // getting audiosource component
   // AudioSource buttonSound;

    //change to MainMenu Scene
    public void LoadMainMenu()
    {
        /*
        //Fetch the AudioSource from the GameObject
        GameObject startButton = GameObject.FindWithTag("StartButton");
        buttonSound = startButton.GetComponent<AudioSource>();
        */
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }
}
