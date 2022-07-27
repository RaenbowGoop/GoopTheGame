using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Gacha Banner Database", menuName = "GachaBannerDatabase/GachaBannerDatabase")]
public class GachaBannerDatabaseObject : ScriptableObject
{
    public GachaBannerObject[] gachaBanners;  // stores all gacha banners
    public string[] currentBanners;  // stores names of current banners available

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }
}