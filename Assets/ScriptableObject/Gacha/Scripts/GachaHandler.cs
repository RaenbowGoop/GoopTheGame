using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GachaHandler : MonoBehaviour
{
    // Player Properties
    public GameObject player;
    private Player playerScript;

    // Banner Properties
    public GachaBannerObject currentBanner;
    public GachaBannerDatabaseObject bannerDatabase;
    public ItemDatabaseObject goopDatabase;

    public List<GoopObject> rateUpSixStarUnits;
    public List<GoopObject> sixStarUnits;
    public List<GoopObject> fiveStarUnits;
    public List<GoopObject> fourStarUnits;

    // Display Objects
    public GameObject insufficientFundsDisplay;
    public GameObject confirmPullDisplay;

    // Pull Results
    List<GoopObject> pullResults;
    System.Random randNumGen;

    Dictionary<string, GameObject> bannerIcons = new Dictionary<string, GameObject>();
    GameObject bannerIconPrefab;
    void Start()
    {
        //loading banner icon prefab
        bannerIconPrefab = Resources.Load<GameObject>("Prefab\\UI\\GachaPrefabs\\BannerIcon");

        //setting currentbanner 
        currentBanner = bannerDatabase.GetBanner[bannerDatabase.currentBanners[0]];

        //fill 4,5,6 star lists
        fillUnitPools();

        //Display Banner
        displayBanner();

        //Display Icons
        displayCurrentBannersIcons();

        //Display Goop Bucks
        playerScript = player.GetComponent<Player>();
        playerScript.setGoopBucks(30000);
        int goopBucks = playerScript.getGoopBucks();
        if(goopBucks > 999999)
        {
            goopBucks = 999999;
        }
        GameObject.FindWithTag("GoopCurrency").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = goopBucks.ToString();

        //Hide Insufficient Funds Display
        hideInsufficientFundsDisplay();

        //Hide Confirmation Pull Display
        hideConfirmPullDisplay();

        //Setting up RNG
        randNumGen = new System.Random();

        //Setting up pull result container
        pullResults = new List<GoopObject>();

        /* FOR PRINTING POOL
        Debug.Log("rateup 6 stars");
        foreach (GoopObject goop in rateUpSixStarUnits)
        {
            Debug.Log(goop.goopFaction + " " + goop.goopName);
        }

        Debug.Log("non rate up 6 stars");
        foreach (GoopObject goop in sixStarUnits)
        {
            Debug.Log(goop.goopFaction + " " + goop.goopName);
        }

        Debug.Log("5 stars");
        foreach (GoopObject goop in fiveStarUnits)
        {
            Debug.Log(goop.goopFaction + " " + goop.goopName);
        }

        Debug.Log("4 stars");
        foreach (GoopObject goop in fourStarUnits)
        {
            Debug.Log(goop.goopFaction + " " + goop.goopName);
        }
        */
    }

    void displayBanner()
    {
        GameObject gachaBannerDisplayGO = GameObject.FindWithTag("GachaBannerDisplay");
        if (currentBanner != null && gachaBannerDisplayGO != null)
        {
            gachaBannerDisplayGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentBanner.bannerDescription;
            gachaBannerDisplayGO.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = currentBanner.bannerArt;
        }
    }

    void displayCurrentBannersIcons()
    {
        GameObject gachaBannerIconDisplayGO = GameObject.FindWithTag("GachaBannerDisplay");
        for (int i = 0; i < bannerDatabase.currentBanners.Length; i++)
        {
            GameObject obj = Instantiate(bannerIconPrefab, Vector3.zero, Quaternion.identity, transform);
            GachaBannerObject banner = bannerDatabase.GetBanner[bannerDatabase.currentBanners[i]];

            obj.transform.localScale = new Vector3(.60f, .60f, .60f);
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = banner.bannerIcon;
            obj.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => changeBanner(banner));
            bannerIcons.Add(bannerDatabase.currentBanners[i], obj);
        }
    }

    void fillUnitPools()
    {
        //clear pools
        rateUpSixStarUnits.Clear();
        sixStarUnits.Clear();
        fiveStarUnits.Clear();
        fourStarUnits.Clear();

        // Standard Banner
        if (currentBanner.type == BannerType.Standard)
        {
            for (int i = 0; i < goopDatabase.GoopObjects.Length; i++)
            {
                if (goopDatabase.GoopObjects[i].goopRarity == 6)
                {
                    sixStarUnits.Add(goopDatabase.GoopObjects[i]);
                }
                else if (goopDatabase.GoopObjects[i].goopRarity == 5)
                {
                    fiveStarUnits.Add(goopDatabase.GoopObjects[i]);
                }
                else
                {
                    fourStarUnits.Add(goopDatabase.GoopObjects[i]);
                }
            }
        }
        // Faction Banner
        else if (currentBanner.type == BannerType.Faction)
        {
            for (int i = 0; i < goopDatabase.GoopObjects.Length; i++)
            {
                if (goopDatabase.GoopObjects[i].goopRarity == 6)
                {
                    if (currentBanner.isRateUp(goopDatabase.GoopObjects[i]))
                    {
                        rateUpSixStarUnits.Add(goopDatabase.GoopObjects[i]);
                    }
                    else
                    {
                        sixStarUnits.Add(goopDatabase.GoopObjects[i]);
                    }
                }
                else if (goopDatabase.GoopObjects[i].goopRarity == 5)
                {
                    fiveStarUnits.Add(goopDatabase.GoopObjects[i]);
                }
                else
                {
                    fourStarUnits.Add(goopDatabase.GoopObjects[i]);
                }
            }
        }
        // Selected Banner
        else if (currentBanner.type == BannerType.Selected)
        {
            for (int i = 0; i < goopDatabase.GoopObjects.Length; i++)
            {
                if (goopDatabase.GoopObjects[i].goopRarity == 6)
                {
                    bool rateUpUnit = currentBanner.isRateUp(goopDatabase.GoopObjects[i]);
                    if (rateUpUnit)
                    {
                        rateUpSixStarUnits.Add(goopDatabase.GoopObjects[i]);
                    }
                    else
                    {
                        sixStarUnits.Add(goopDatabase.GoopObjects[i]);
                    }
                }
                else if (goopDatabase.GoopObjects[i].goopRarity == 5)
                {
                    fiveStarUnits.Add(goopDatabase.GoopObjects[i]);
                }
                else
                {
                    fourStarUnits.Add(goopDatabase.GoopObjects[i]);
                }
            }
        }
    }

    void changeBanner(GachaBannerObject banner)
    {
        currentBanner = banner;
        fillUnitPools();
        displayBanner();
    }

    public void showInsufficientFundsDisplay()
    {
        insufficientFundsDisplay.SetActive(true);
    }

    public void hideInsufficientFundsDisplay()
    {
        insufficientFundsDisplay.SetActive(false);

    }

    public void showConfirmPullDisplay(int numberOfPulls)
    {
        if (playerScript.getGoopBucks() - 300*numberOfPulls < 0)
        {
            showInsufficientFundsDisplay();
        }
        else
        {
            confirmPullDisplay.SetActive(true);
            confirmPullDisplay.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>()
                .text = "Spend " + numberOfPulls * 300 + " GoopBucks\n" + "to perform x" + numberOfPulls + " pull";
            confirmPullDisplay.transform.GetChild(1).GetChild(0)
                .GetComponent<Button>().onClick.AddListener(() => pullGoop(numberOfPulls));
        }
    }

    public void hideConfirmPullDisplay()
    {
        confirmPullDisplay.SetActive(false);
        confirmPullDisplay.transform.GetChild(1).GetChild(0)
                .GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void pullGoop(int numOfPulls)
    {
        hideConfirmPullDisplay();

        pullResults.Clear();
        playerScript.subtractGoopBucks(300 * numOfPulls);
        GameObject.FindWithTag("GoopCurrency").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerScript.getGoopBucks().ToString();
        
        for  (int i = 0; i < numOfPulls; i++)
        {
            int num = randNumGen.Next(1, 101);
            int index = -1;
            if (num <= currentBanner.rate4Star)
            {
                index = randNumGen.Next(0, fourStarUnits.Count);
                pullResults.Add(fourStarUnits[index]);
                playerScript.inventory.addItem(fourStarUnits[index]);
            }
            else if (num <= currentBanner.rate4Star + currentBanner.rate5Star)
            {
                index = randNumGen.Next(0, fiveStarUnits.Count);
                pullResults.Add(fiveStarUnits[index]);
                playerScript.inventory.addItem(fiveStarUnits[index]);
            }
            else
            {
                if (rateUpSixStarUnits.Count == 0)
                {
                    index = randNumGen.Next(0, sixStarUnits.Count);
                    pullResults.Add(sixStarUnits[index]);
                    playerScript.inventory.addItem(sixStarUnits[index]);
                }
                else
                {
                    num = randNumGen.Next(1, 3);
                    if (num == 1)
                    {
                        index = randNumGen.Next(0, sixStarUnits.Count);
                        pullResults.Add(sixStarUnits[index]);
                        playerScript.inventory.addItem(sixStarUnits[index]);
                    }
                    else
                    {
                        index = randNumGen.Next(0, rateUpSixStarUnits.Count);
                        pullResults.Add(rateUpSixStarUnits[index]);
                        playerScript.inventory.addItem(rateUpSixStarUnits[index]);
                    }
                }
            }
        }

        foreach(GoopObject item in pullResults)
        {
            Debug.Log(item.goopFaction + " " + item.goopName);
        }
    }
}