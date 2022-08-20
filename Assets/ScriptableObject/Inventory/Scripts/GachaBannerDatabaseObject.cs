using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Gacha Banner Database", menuName = "GachaBannerDatabase/GachaBannerDatabase")]
public class GachaBannerDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public GachaBannerObject[] gachaBanners;  // stores all gacha banners
    public string[] currentBanners;  // stores names of current banners available

    public Dictionary<string, GachaBannerObject> GetBanner = new Dictionary<string, GachaBannerObject>();

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {

    }

    public void Awake()
    {
        GetBanner = new Dictionary<string, GachaBannerObject>();
        for (int i = 0; i < gachaBanners.Length; i++)
        {
            GetBanner.Add(gachaBanners[i].bannerName, gachaBanners[i]);
        }
    }
}
