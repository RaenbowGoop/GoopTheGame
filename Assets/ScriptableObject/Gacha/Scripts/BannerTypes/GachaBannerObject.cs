using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BannerType
{
    Standard,
    Faction,
    Selected
}

public abstract class GachaBannerObject : ScriptableObject
{
    public BannerType type;
    public string bannerName;
    public string bannerDescription;
    public int rate6Star = 10;
    public int rate5Star = 25;
    public int rate4Star = 65;

    public Sprite bannerArt;
    public Sprite bannerIcon;

    public abstract bool isRateUp(GoopObject goop);
}
