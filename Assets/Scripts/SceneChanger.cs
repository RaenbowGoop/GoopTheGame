using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; //add to use scene manager
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
   

    //change to MainMenu Scene
    public void LoadMainMenu()
    {
        //Making Start Menu BGM stop
        // getting audiosource component
        AudioSource StartMenuMusic;
        //Fetch the AudioSource from the GameObject
        GameObject musicObject = GameObject.FindWithTag("StartMenuBGM");
        StartMenuMusic = musicObject.GetComponent<AudioSource>();
        Destroy(StartMenuMusic);

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
