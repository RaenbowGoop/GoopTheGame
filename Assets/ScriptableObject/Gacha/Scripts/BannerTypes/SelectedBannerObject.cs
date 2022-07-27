using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Selected Banner Object", menuName = "GachaBanner/SelectedBanner")]
public class SelectedBannerObject : GachaBannerObject
{
    public int[] rateUpUnitIDs;

    void OnAwake()
    {
        type = BannerType.Selected;
    }
}
