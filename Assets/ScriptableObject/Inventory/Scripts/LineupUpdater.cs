using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineupUpdater : MonoBehaviour
{
    [SerializeField] InventoryObject lineup;
    [SerializeField] InventoryObject playerInventory;

    public void UpdateLineup()
    {
        for(int l = 0; l < lineup.container.Items.Count; l++)
        {
            for(int i = 0; i < playerInventory.container.Items.Count; i++)
            {
                if(lineup.container.Items[l].item.Equals(playerInventory.container.Items[i].item))
                {
                    lineup.removeItem(lineup.container.Items[l]);
                    lineup.addItem(playerInventory.container.Items[i].item);
                }
            }
        }
    }
}
