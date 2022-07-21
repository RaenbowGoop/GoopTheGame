using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{

    public string savePath;
    public ItemDatabaseObject database;
    public Inventory container;
    private IDataService dataService = new JSonDataService();

    public void addItem(GoopObject _item)
    {
        Debug.Log("adding new unit");
        for (int count = 0; count < container.Items.Count; count++)
        {
            //returns from function if object is already in container
            if (container.Items[count].item.goopFaction == _item.goopFaction && container.Items[count].item.goopName == _item.goopName)
            {
                container.Items[count].item.goopDuplicates++;
                container.Items.Sort();
                Save();
                Load();
                return; 
            }
        }
        Debug.Log("No dups");
        container.Items.Add(new InventorySlot(database.GetId[_item], _item));
        container.Items.Sort();
        Save();
        Load();
    }

    public void sortInventoryDefault()
    {
        container.Items.Sort();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        //SerializeJson
        if (!dataService.SaveData(savePath,container))
        {
            Debug.LogError("Couldn't save file!");
        }
    }

    [ContextMenu("Load")]
    public void Load()
    {
        try
        {
            container = dataService.LoadData<Inventory>(savePath);
            if (container == null)
            {
                container = new Inventory();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Could not read file!");
        }

        for (int i = 0; i < container.Items.Count; i++)
        {
            container.Items[i].item.uiDisplay = Resources.Load<Sprite>(container.Items[i].item.uiDisplayPath);
            container.Items[i].item.prefab = Resources.Load<GameObject>(container.Items[i].item.prefabPath);
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }
}

[JsonObject(MemberSerialization.OptIn)]
[System.Serializable]
public class InventorySlot : IComparable<InventorySlot>
{
    [JsonProperty] public int ID;
    [JsonProperty] public GoopObject item;
    public InventorySlot(int _id, GoopObject _item)
    {
        ID = _id;
        item = _item;
    }

    public int CompareTo(InventorySlot other)
    {
        return this.item.CompareGoopObjectDefault(other.item);
    }
}

[JsonObject(MemberSerialization.OptIn)]
[System.Serializable]
public class Inventory
{
    [JsonProperty] public List<InventorySlot> Items = new List<InventorySlot>();
}
