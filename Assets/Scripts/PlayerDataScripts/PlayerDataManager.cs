using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour
{
    public Toggle musicTog;
    public Toggle SFXTog;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("musicOn"))
        {
            PlayerPrefs.SetInt("musicOn", 1);
        }

        if (!PlayerPrefs.HasKey("SFXOn"))
        {
            PlayerPrefs.SetInt("SFXOn", 1);
        }
    }

    public void saveOptions()
    {
        if (musicTog.isOn)
        {
            PlayerPrefs.SetInt("musicOn", 1);
            toggleAudioSource("BGM", false);
        }
        else
        {
            PlayerPrefs.SetInt("musicOn", -1);
            toggleAudioSource("BGM", true);
        }

        if (SFXTog.isOn)
        {
            PlayerPrefs.SetInt("SFXOn", 1);
            toggleAudioSource("SFX", false);
        }
        else
        {
            PlayerPrefs.SetInt("SFXOn", -1);
            toggleAudioSource("SFX", true);
        }
    }

    void toggleAudioSource(string tag, bool isOff)
    {
        GameObject[] audioGOs = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < audioGOs.Length; i++)
        {
            audioGOs[i].GetComponent<AudioSource>().mute = isOff;
        }
    }
}
