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

    private void OnEnable()
    {
        inventory.Save();
        inventory.Load();
        //if the inventory is empty at start, add basic units
        if (inventory.container.Items.Count < 4)
        {
            
            inventory.addItem(inventory.database.GoopObjects[0]);
            inventory.addItem(inventory.database.GoopObjects[1]);
            inventory.addItem(inventory.database.GoopObjects[2]);
            inventory.addItem(inventory.database.GoopObjects[3]);

            for (int i = 0; i < 4; i++)
            {
                inventory.database.GoopObjects[i].ResetValues();
            }
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
