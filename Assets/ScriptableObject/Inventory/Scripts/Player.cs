using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    private int numOfGoopPotions;
    private int numOfGoopBucks;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<Item>();
        if (item)
        {
            inventory.addItem(item.item);
            Destroy(collision.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        //saving inventory
        inventory.Save();
        inventory.container.Items.Clear();

        //saving currency
        PlayerPrefs.SetInt("numOfGoopPotions", numOfGoopPotions);
        PlayerPrefs.SetInt("numOfGoopBucks", numOfGoopBucks);
    }

    private void Start()
    {
        //LOADING INVENTORY
        inventory.Load();
        //if the inventory is empty at start, add basic units
        if (inventory.container.Items.Count < 4)
        {
            inventory.Clear();
            for (int i = 0; i < 4; i++)
            {
                inventory.database.GoopObjects[i].ResetValues();
                inventory.addItem(inventory.database.GoopObjects[i]);
            }
        }
        inventory.sortInventoryDefault();
        inventory.Save();

        //LOADING CURRENCY
        numOfGoopBucks = PlayerPrefs.GetInt("numOfGoopBucks", 0);
        numOfGoopPotions = PlayerPrefs.GetInt("numOfGoopPotions", 0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            inventory.Save();
            inventory.Load();
        }
    }

    public int getGoopPotions()
    {
        return numOfGoopPotions;
    }

    public int getGoopBucks()
    {
        return numOfGoopBucks;
    }

    public void addGoopPotions(int amount)
    {
        numOfGoopPotions += amount;
    }

    public void addGoopBucks(int amount)
    {
        numOfGoopBucks += amount;
    }

    public void subtractGoopPotions(int amount)
    {
        numOfGoopPotions -= amount;
    }

    public void subtractGoopBucks(int amount)
    {
        numOfGoopBucks -= amount;
    }
}
