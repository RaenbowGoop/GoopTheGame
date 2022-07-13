using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Goop,
    Item,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)] 
    public string description;
}
