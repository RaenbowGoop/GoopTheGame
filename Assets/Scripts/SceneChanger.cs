using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; //add to use scene manager
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public Animator sceneTransition;
    public float transitionTime = 3f;

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

        StartCoroutine(LoadLevel("MainMenu"));
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu"); 
    }

    public void UnloadOptions()
    {
        SceneManager.UnloadScene("OptionsMenu");
    }

    public void LoadStartMenuWTransition()
    {
        AudioSource MainMenuMusic;
        //Fetch the AudioSource from the GameObject
        GameObject musicObject = GameObject.FindWithTag("MainMenuBGM");
        MainMenuMusic = musicObject.GetComponent<AudioSource>();
        Destroy(MainMenuMusic);

        StartCoroutine(LoadLevel("MainMenuBGM"));
    }

    //function to quit game
    public void QuitGame()
    {
        Debug.Log("quit"); //for testing, delete later
        Application.Quit();
    }

    IEnumerator LoadLevel(string sceneName)
    {
        //Play animation
        sceneTransition.SetTrigger("Start");

        //wait for transition to finish
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadScene(sceneName);
    }
}
