using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item Database", menuName ="Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public GoopObject[] GoopObjects;

    public Dictionary<GoopObject, int> GetId = new Dictionary<GoopObject, int>();
    public Dictionary<int, GoopObject> GetGoop = new Dictionary<int, GoopObject>();

    //functions you can put code in that fire before and after unity serializes objects

    public void OnAfterDeserialize()
    {
        //clears dictionary for reloading
        GetId = new Dictionary<GoopObject, int>();
        GetGoop = new Dictionary<int, GoopObject>();
        for (int i = 0; i < GoopObjects.Length; i++)
        {
            GetId.Add(GoopObjects[i], i);
            GetGoop.Add(i, GoopObjects[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }

    public GoopObject getMatchingGoop(GoopObject goopItem)
    {
        foreach(var item in GetId)
        {
            if(goopItem.Equals(item.Key))
            {
                return item.Key;
            }
        }
        return null;
    }
}
