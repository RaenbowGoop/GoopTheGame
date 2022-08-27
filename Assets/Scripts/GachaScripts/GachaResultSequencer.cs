using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class GachaResultSequencer : MonoBehaviour
{
    public GameObject gachaResultsPageUI;
    
    [SerializeField] AudioSource openingAnimationBGM;
    [SerializeField] GameObject openingAnimationGO;
    [SerializeField] GameObject petalAnimatorGO;
    [SerializeField] GameObject starsAnimatorGO;
    [SerializeField] GameObject wheelAnimatorGO;

    VideoPlayer openingAnimation;
    Animator petalAnimator;
    Animator starsAnimator;
    Animator wheelAnimator;

    string currentPetalState;
    string currentStarsState;
    string currentWheelState;

    bool introIsOver;
    bool unitAnimationIsOver;
    bool isShowingAllResults;

    [SerializeField] GameObject unitArtGO;
    [SerializeField] GameObject skipAllButton;
    [SerializeField] GameObject skipSingleButton;
    [SerializeField] GameObject BackToGachaButton;
    Animator spriteAnimation;

    List<GoopObject> units;
    int currentIndex;

    float time;
    float timeDelay;

    void Start() 
    {
        //initializing flags and other primitive types
        time = 0f;
        timeDelay = 10f;
        currentIndex = 0;
        Time.timeScale = 1;

        introIsOver = false;
        unitAnimationIsOver = false;
        isShowingAllResults = false;

        //getting list of units form pull results
        units = GameObject.FindWithTag("PullResults").GetComponent<PullResults>().pullResults;

        //hide final gacha results page
        gachaResultsPageUI.SetActive(false);

        //hide unit icon for animation
        unitArtGO.SetActive(false);

        //setting animators
        petalAnimator = petalAnimatorGO.GetComponent<Animator>();
        starsAnimator = starsAnimatorGO.GetComponent<Animator>();
        wheelAnimator =wheelAnimatorGO.GetComponent<Animator>();
        spriteAnimation = unitArtGO.GetComponent<Animator>();

        // Find highest rarity in pull results and sets opening animation accordingly
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

        //play Opening animation
        openingAnimation.Play();
        openingAnimationBGM.Play();

        //will set introIsOver to true when the openingAnimation has played through
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
                //hide everything while script figures out elements for next unit
                if(petalAnimatorGO != null)
                {
                    petalAnimatorGO.SetActive(false);
                }
                if (starsAnimatorGO != null)
                {
                    starsAnimatorGO.SetActive(false);
                }
                if (wheelAnimatorGO != null)
                {
                    wheelAnimatorGO.SetActive(false);
                }

                //Set unit's icon
                unitArtGO.transform.GetChild(0).GetComponent<Image>().sprite = units[currentIndex].uiDisplay;

                //Set unit's petal and stars animations based on rarity
                if (units[currentIndex].goopRarity == 6)
                {
                    currentPetalState = "6StarPetalTransition";
                    currentStarsState = "6StarsAnimation";
                    currentWheelState = "6StarWheelAnimation";
                }
                else if (units[currentIndex].goopRarity == 5)
                {
                    currentPetalState = "5StarPetalTransition";
                    currentStarsState = "5StarsAnimation";
                    currentWheelState = "5StarWheelAnimation";
                }
                else
                {
                    currentPetalState = "4StarPetalTransition";
                    currentStarsState = "4StarsAnimation";
                    currentWheelState = "4StarWheelAnimation";
                }

                //reveal all visual elements
                petalAnimatorGO.SetActive(true);
                starsAnimatorGO.SetActive(true);
                wheelAnimatorGO.SetActive(true);
                unitArtGO.SetActive(true);

                //Play animation
                petalAnimator.Play(currentPetalState);
                starsAnimator.Play(currentStarsState);
                wheelAnimator.Play(currentWheelState);
                spriteAnimation.Play("Base Layer.UnitFade", 0, 0f);

                //increment timer
                time = time + 1f * Time.deltaTime;
            }
            else if (time >= timeDelay)
            {
                //hide all of the elements
                petalAnimatorGO.SetActive(false);
                starsAnimatorGO.SetActive(false);
                wheelAnimatorGO.SetActive(false);
                unitArtGO.SetActive(false);
                
                //reset timer
                time = 0;
                
                //if there are no more units to display, unitAnimationIsOver flag to true 
                currentIndex = currentIndex + 1;
                if (currentIndex >= units.Count)
                {
                    unitAnimationIsOver = true;
                }
            }
            else
            {
                //increment timer
                time = time + 1f * Time.deltaTime;
            }
        }
        else if (unitAnimationIsOver && !isShowingAllResults)
        {
            //hide skipping buttons
            skipAllButton.GetComponent<Image>().enabled = false;
            skipSingleButton.GetComponent<Image>().enabled = false;
            skipAllButton.GetComponent<Button>().enabled = false;
            skipSingleButton.GetComponent<Button>().enabled = false;
            skipAllButton.transform.GetChild(0). GetComponent<TextMeshProUGUI>().enabled = false;

            //reveal button to return to gacha menu
            BackToGachaButton.GetComponent<Image>().enabled = true;
            BackToGachaButton.GetComponent<Button>().enabled = true;

            //DISPLAY PULL RESULTS ON LAST SCREEN
            isShowingAllResults = true;
            gachaResultsPageUI.SetActive(true);
            GameObject.FindWithTag("PullResults").GetComponent<PullResults>().displayGachaResults();

        }
    }

    public void skipAnimation()
    {
        //if player is skipping during opening animation, then stop opening animation
        if (!introIsOver && openingAnimation != null)
        {
            openingAnimation.Stop();
            openingAnimationBGM.Stop();
            introIsOver = true;
        }

        //make user wait 1 second before being able to skip
        if(time > 1)
        {
            time = timeDelay;
        }
    }
    public void skipAllAnimations()
    {
        //stop opening animation 
        if (!introIsOver && openingAnimation != null)
        {
            openingAnimation.Stop();
            openingAnimationBGM.Stop();
            introIsOver = true;
        }
        //set unitAnimationIsOver flag to true
        unitAnimationIsOver = true;
    }
}