using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item Database", menuName ="Inventory System/Items/YenDatabase")]
public class YenDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public YenObject[] YenObject;
    public Dictionary<int, YenObject> GetYen = new Dictionary<int, YenObject>();

    //functions you can put code in that fire before and after unity serializes objects

    public void OnAfterDeserialize()
    {
        //clears dictionary for reloading
        GetYen = new Dictionary<int, YenObject>();
        for (int i = 0; i < YenObject.Length; i++)
        {
            GetYen.Add(i, YenObject[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }
}
