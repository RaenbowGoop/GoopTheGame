using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum ItemType
{
    Goop,
    Yen
}

[JsonObject(MemberSerialization.OptIn)]
public abstract class ItemObject : ScriptableObject
{
    public Sprite uiDisplay; //for inventory
    //public GameObject inventoryPrefab;
    [JsonProperty] public ItemType type;
    [TextArea(15, 20)]
    [JsonProperty] public string description;
    [TextArea(1, 2)]
    [JsonProperty] public string uiDisplayPath;
}
