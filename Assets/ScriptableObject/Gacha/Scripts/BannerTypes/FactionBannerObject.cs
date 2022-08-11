using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Faction Banner Object", menuName = "GachaBanner/FactionBanner")]
public class FactionBannerObject : GachaBannerObject
{
    public string bannerFaction;

    void Awake()
    {
        type = BannerType.Faction;
    }

    public override bool isRateUp(GoopObject goop)
    {
        return goop.goopFaction == bannerFaction;
    }
}
