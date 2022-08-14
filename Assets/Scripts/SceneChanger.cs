using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; //add to use scene manager
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public Animator sceneTransition;
    float transitionTime = 1f;

    //Loading Level with transition

    IEnumerator LoadLevel(string sceneName)
    {
        //Play animation
        sceneTransition.SetTrigger("Start");

        //wait for transition to finish
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadScene(sceneName);
    }

    //change to Start Menu Scene
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

    //change to Options Scene
    public void LoadOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    //change to MainMenu Scene
    public void LoadMainMenu()
    {
        GameObject audio = GameObject.Find("StartMenuBGM");
        SceneManager.MoveGameObjectToScene(audio, SceneManager.GetSceneByName("StartMenu"));
        Destroy(audio);

        StartCoroutine(LoadLevel("MainMenu"));
    }

    public void LoadGachaMenuFromGachaResults()
    {
        GameObject obj = GameObject.FindWithTag("PullResults");
        Destroy(obj);

        StartCoroutine(LoadLevel("GachaMenu"));
    }

    public void LoadGachaMenuFromMainMenu()
    {
        GameObject audio = GameObject.Find("MainMenuBGM");
        Destroy(audio);

        StartCoroutine(LoadLevel("GachaMenu"));
    }

    public void LoadMainMenuFromGachaMenu()
    {
        GameObject obj = GameObject.FindWithTag("PullResults");
        Destroy(obj);

        StartCoroutine(LoadLevel("MainMenu"));
    }

    //function to quit game
    public void QuitGame()
    {
        Application.Quit();
    }

    //change to sceneName
    public void LoadScene( string sceneName )
    {
        StartCoroutine(LoadLevel(sceneName));
    }
}
