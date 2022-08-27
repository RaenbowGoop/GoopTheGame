using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
          
    }

    public void SetSceneSounds()
    {
        if (PlayerPrefs.GetInt("musicOn") == -1)
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            foreach (GameObject audio in BGM)
            {
                audio.GetComponent<AudioSource>().mute = true;
            }
        }

        if (PlayerPrefs.GetInt("SFXOn") == -1)
        {
            GameObject[] SFX = GameObject.FindGameObjectsWithTag("SFX");
            foreach (GameObject audio in SFX)
            {
                audio.GetComponent<AudioSource>().mute = true;
            }
        }
    }

    void Update()
    {
        SetSceneSounds();
    }
}
