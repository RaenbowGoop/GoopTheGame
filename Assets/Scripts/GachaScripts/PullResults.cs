using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PullResults : MonoBehaviour
{
    public List<GoopObject> pullResults;
    public List<bool> goopObjectIsNew;
    public int highestRarity;

    //Goop Gacha Prefabs
    public GameObject fourStarGachaPrefab;
    public GameObject fiveStarGachaPrefab;
    public GameObject sixStarGachaPrefab;

    //GameObject Parent
    GameObject parent;

    //Current Banner
    public GachaBannerObject currentBanner;

    void Start()
    {
        goopObjectIsNew = new List<bool>();
        pullResults = new List<GoopObject>();
    }

    public void displayGachaResults()
    {
        parent = GameObject.FindWithTag("UnitDisplay");
        GameObject obj;
        for (int i = 0; i < pullResults.Count; i++)
        {
            if (pullResults[i].goopRarity == 6)
            {
                obj = Instantiate(sixStarGachaPrefab, Vector3.zero, Quaternion.identity, parent.transform.GetChild(0).transform);
            }
            else if (pullResults[i].goopRarity == 5)
            {
                obj = Instantiate(fiveStarGachaPrefab, Vector3.zero, Quaternion.identity, parent.transform.GetChild(0).transform);
            }
            else
            {
                obj = Instantiate(fourStarGachaPrefab, Vector3.zero, Quaternion.identity, parent.transform.GetChild(0).transform);
            }

            //Set Goop Image
            obj.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = pullResults[i].uiDisplay;
            //obj.transform.localScale = Vector3.one * .5f;

            if ( !goopObjectIsNew[i])
            {
                obj.transform.GetChild(4).GetComponent<Image>().enabled = false;
            }
        }
    }

    public void setCurrentBanner(GachaBannerObject gachaBanner)
    {
        currentBanner = gachaBanner;
    }

    public GachaBannerObject getCurrentBanner()
    {
        return currentBanner;
    }
}
