using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonStatus : MonoBehaviour
{
    public Toggle musicTog;
    public Toggle SFXTog;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("musicOn") == -1)
        {
            musicTog.isOn = false;
        }

        if (PlayerPrefs.GetInt("SFXOn") == -1)
        {
            SFXTog.isOn = false;
        }

    }
}
