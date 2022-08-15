using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class GachaResultSequencer : MonoBehaviour
{
    public GameObject openingAnimationGO;
    public VideoPlayer petalTransition;
    public GameObject backgroundAnimationGO;

    VideoPlayer openingAnimation;
    VideoPlayer backgroundAnimation;

    bool introIsOver;
    bool unitAnimationIsOver;
    bool isShowingAllResults;

    public GameObject unitArtGO;
    public GameObject skipAllButton;
    public GameObject skipSingleButton;
    public GameObject BackToGachaButton;
    Animator spriteAnimation;

    List<GoopObject> units;
    int currentIndex;

    float time;
    float timeDelay;

    void Start() 
    {
        time = 0f;
        timeDelay = 12f;

        currentIndex = 0;

        spriteAnimation = unitArtGO.GetComponent<Animator>();
        unitArtGO.SetActive(false);
        units = GameObject.FindWithTag("PullResults").GetComponent<PullResults>().pullResults;

        introIsOver = false;
        unitAnimationIsOver = false;
        isShowingAllResults = false;

        int highestRarity = GameObject.FindWithTag("PullResults").GetComponent<PullResults>().highestRarity;
        if (highestRarity == 6)
        {
            openingAnimation = openingAnimationGO.transform.GetChild(0).GetComponent<VideoPlayer>();
        }
        else if (highestRarity == 5)
        {
            openingAnimation = openingAnimationGO.transform.GetChild(1).GetComponent<VideoPlayer>();
        }
        else
        {
            openingAnimation = openingAnimationGO.transform.GetChild(2).GetComponent<VideoPlayer>();
        }
        openingAnimation.Play();
        openingAnimation.loopPointReached += CheckOver; 

    }

    void CheckOver(VideoPlayer vp)
    {
        introIsOver = true;
    }

    void Update()
    {
        if (introIsOver && !unitAnimationIsOver)
        {
            if (time == 0)
            {
                unitArtGO.SetActive(true);
                unitArtGO.transform.GetChild(0).GetComponent<Image>().sprite = units[currentIndex].uiDisplay;

                if(units[currentIndex].goopRarity == 6)
                {
                    backgroundAnimation = backgroundAnimationGO.transform.GetChild(0).GetComponent<VideoPlayer>();
                }
                else if (units[currentIndex].goopRarity == 5)
                {
                    backgroundAnimation = backgroundAnimationGO.transform.GetChild(1).GetComponent<VideoPlayer>();
                }
                else
                {
                    backgroundAnimation = backgroundAnimationGO.transform.GetChild(2).GetComponent<VideoPlayer>();
                }

                //Play animation
                petalTransition.Play();
                backgroundAnimation.Play();
                spriteAnimation.Play("Base Layer.UnitFade", 0, 0f);

                time = time + 1f * Time.deltaTime;
            }
            else if (time >= timeDelay)
            {
                petalTransition.Stop();
                backgroundAnimation.Stop();
                time = 0;
                unitArtGO.SetActive(false);
                currentIndex = currentIndex + 1;
                if (currentIndex >= units.Count)
                {
                    unitAnimationIsOver = true;
                }
            }
            else
            {
                time = time + 1f * Time.deltaTime;
            }
        }
        else if (unitAnimationIsOver && !isShowingAllResults)
        {
            skipAllButton.GetComponent<Image>().enabled = false;
            skipSingleButton.GetComponent<Image>().enabled = false;
            skipAllButton.GetComponent<Button>().enabled = false;
            skipSingleButton.GetComponent<Button>().enabled = false;
            skipAllButton.transform.GetChild(0). GetComponent<TextMeshProUGUI>().enabled = false;

            BackToGachaButton.GetComponent<Image>().enabled = true;
            BackToGachaButton.GetComponent<Button>().enabled = true;

            //DISPLAY PULL RESULTS ON LAST SCREEN

            isShowingAllResults = true;
        }
    }

    public void skipAnimation()
    {
        if (!introIsOver)
        {
            openingAnimation.Stop();
            introIsOver = true;
        }
        if(time > 1)
        {
            time = timeDelay;
        }
    }
    public void skipAllAnimations()
    {
        if (!introIsOver)
        {
            openingAnimation.Stop();
            introIsOver = true;
        }
        time = timeDelay;
        currentIndex = units.Count - 1;
    }
}