using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Standard Banner Object", menuName = "GachaBanner/StandardBanner")]
public class StandardBannerObject : GachaBannerObject
{
    void OnAwake()
    {
        type = BannerType.Standard;
    }
}
