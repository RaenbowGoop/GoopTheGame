using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplaySettingsScript : MonoBehaviour
{
    [SerializeField] float[] speedMultipliers;
    int currentSpeedindex = 0;

    [SerializeField] GameObject playPauseButton;
    [SerializeField] Sprite playButton;
    [SerializeField] Sprite playButtonPushed;
    [SerializeField] Sprite pauseButton;
    [SerializeField] Sprite pauseButtonPushed;

    [SerializeField] GameObject optionsMenu;

    [SerializeField] GameObject gameSpeedButton;

    public void changePlayState()
    {
        if (Time.timeScale == speedMultipliers[currentSpeedindex])
        {
            Time.timeScale = 0;

            SpriteState spriteState = new SpriteState();
            spriteState.pressedSprite = playButtonPushed;
            playPauseButton.GetComponent<Image>().sprite = playButton;
            playPauseButton.GetComponent<Button>().spriteState = spriteState;

            //transparentScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = speedMultipliers[currentSpeedindex];

            SpriteState spriteState = new SpriteState();
            spriteState.pressedSprite = pauseButtonPushed;
            playPauseButton.GetComponent<Image>().sprite = pauseButton;
            playPauseButton.GetComponent<Button>().spriteState = spriteState;

            //transparentScreen.SetActive(false);
        }
    }

    public void changeSpeed()
    {
        currentSpeedindex++;
        if(currentSpeedindex >= speedMultipliers.Length)
        {
            currentSpeedindex = 0;
        }
        if (Time.timeScale != 0)
        {
            Time.timeScale = speedMultipliers[currentSpeedindex];
        }

        //change Button Display Number
        gameSpeedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + speedMultipliers[currentSpeedindex];
        gameSpeedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "x" + speedMultipliers[currentSpeedindex];
    }

    public void displayOptionsMenu()
    {
        Time.timeScale = 0;

        //enable options menu
        optionsMenu.SetActive(true);
        optionsMenu.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void closeOptionsMenu()
    {
        Time.timeScale = speedMultipliers[currentSpeedindex];

        //enable options menu
        optionsMenu.SetActive(false);
        optionsMenu.transform.GetChild(0).gameObject.SetActive(false);
        optionsMenu.transform.GetChild(1).gameObject.SetActive(false);
        optionsMenu.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void displaySettings()
    {
        optionsMenu.transform.GetChild(0).gameObject.SetActive(false);
        optionsMenu.transform.GetChild(2).gameObject.SetActive(true);

    }

    public void closeSettings()
    {
        optionsMenu.transform.GetChild(0).gameObject.SetActive(true);
        optionsMenu.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void displayLeaveConfirmation()
    {
        optionsMenu.transform.GetChild(0).gameObject.SetActive(false);
        optionsMenu.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void closeLeaveConfirmation()
    {
        optionsMenu.transform.GetChild(0).gameObject.SetActive(true);
        optionsMenu.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ExitStage()
    {
        GameObject.FindWithTag("PlayerBase").transform.parent.GetComponent<PlayerBase>().KillAll();
        GameObject.FindWithTag("EnemyBase").transform.parent.GetComponent<EnemyBase>().KillAll();
        optionsMenu.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
