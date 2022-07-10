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
        GameObject audio = GameObject.Find("StartMenuBGM");
        SceneManager.MoveGameObjectToScene(audio, SceneManager.GetSceneByName("StartMenu"));
        Destroy(audio);

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

    public void LoadStartMenuWTransition()
    {
        GameObject audio = GameObject.Find("MainMenuBGM");
        SceneManager.MoveGameObjectToScene(audio, SceneManager.GetSceneByName("MainMenu"));
        Destroy(audio);

        StartCoroutine(LoadLevel("StartMenu"));


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
