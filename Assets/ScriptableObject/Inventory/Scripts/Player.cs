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
