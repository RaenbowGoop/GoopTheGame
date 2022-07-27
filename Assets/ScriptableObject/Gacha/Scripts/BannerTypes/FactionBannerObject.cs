using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Faction Banner Object", menuName = "GachaBanner/FactionBanner")]
public class FactionBannerObject : GachaBannerObject
{
    public string bannerFaction;

    void OnAwake()
    {
        type = BannerType.Faction;
    }
}
