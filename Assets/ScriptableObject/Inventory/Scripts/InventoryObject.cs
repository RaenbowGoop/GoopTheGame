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
        for(int count = 0; count < container.Items.Count; count++)
        {
            //returns from function if object is already in container
            if (container.Items[count].item.goopFaction == _item.goopFaction && container.Items[count].item.goopName == _item.goopName)
            {
                container.Items[count].item.goopDuplicates++;
                Save();
                return; 
            }
        }
        container.Items.Add(new InventorySlot(database.GetId[_item], _item));
        Save();
 
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
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public GoopObject item;
    public InventorySlot(int _id, GoopObject _item)
    {
        ID = _id;
        item = _item;
    }
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}