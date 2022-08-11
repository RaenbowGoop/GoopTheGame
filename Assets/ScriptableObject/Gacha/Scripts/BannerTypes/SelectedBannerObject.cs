using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Selected Banner Object", menuName = "GachaBanner/SelectedBanner")]
public class SelectedBannerObject : GachaBannerObject
{
    public string[] rateUpUnitTitles;

    void Awake()
    {
        type = BannerType.Selected;
    }

    public override bool isRateUp(GoopObject goop)
    {
        // Back to the Basics: Zhen
        string goopTitle = "[" + goop.goopFaction + "] " + goop.goopName;
        foreach (string title in rateUpUnitTitles)
        {
            if(goopTitle == title)
            {
                return true;
            }
        }
        return false;
    }
}
