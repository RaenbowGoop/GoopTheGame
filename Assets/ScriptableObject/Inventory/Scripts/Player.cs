using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;

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
        inventory.Save();
        inventory.container.Items.Clear();
    }

    private void Start()
    {
        inventory.Load();
        //if the inventory is empty at start, add basic units
        if (SceneManager.GetActiveScene().name == "StartMenu" && inventory.container.Items.Count == 0)
        {
            inventory.addItem(inventory.database.GoopObjects[0]);
            inventory.addItem(inventory.database.GoopObjects[1]);
            inventory.addItem(inventory.database.GoopObjects[2]);
            inventory.addItem(inventory.database.GoopObjects[3]);
            inventory.Save();
            inventory.Load();
            Debug.Log("added units");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            inventory.Save();
            inventory.Load();
        }
    }
}
