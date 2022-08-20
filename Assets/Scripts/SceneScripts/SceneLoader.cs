using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //function to quit game
    public void QuitGame()
    {
        Debug.Log("quit"); //for testing, delete later
        Application.Quit();
    }
}
