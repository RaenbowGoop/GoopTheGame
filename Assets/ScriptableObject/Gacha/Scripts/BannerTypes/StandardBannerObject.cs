using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Standard Banner Object", menuName = "GachaBanner/StandardBanner")]
public class StandardBannerObject : GachaBannerObject
{
    void Awake()
    {
        type = BannerType.Standard;
    }

    public override bool isRateUp(GoopObject goop)
    {
        return false;
    }
}
