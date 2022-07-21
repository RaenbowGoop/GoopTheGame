using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        //Creates GoopObjects, sets UI values, and Adds GoopObjects to ItemsDisplayed
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {

            InventorySlot slot = inventory.container.Items[i];
            int totalLevel = slot.item.goopLevel + slot.item.goopDuplicates;

            GameObject obj = Instantiate(slot.item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = slot.item.uiDisplay;
            obj.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");
            itemsDisplayed.Add(slot, obj);

            if (totalLevel >= slot.item.goopLevelCap)
            {
                Color c = new Color(255, 240, 0, 1.0f);
                itemsDisplayed[slot].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().color = c;
            }
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {
            InventorySlot slot = inventory.container.Items[i];
            int totalLevel = slot.item.goopLevel + slot.item.goopDuplicates;

            InventorySlot key = findInventorySlotMatch(slot);
            if (key == null)
            {
                GameObject obj = Instantiate(slot.item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = slot.item.uiDisplay;
                obj.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");
                itemsDisplayed.Add(slot, obj);

                if (totalLevel >= slot.item.goopLevelCap)
                {
                    Color c = new Color(222, 0, 0, 1.0f);
                    obj.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().color = c;
                }
            }
            else //If object exists in ItemsDisplayed, update UI 
            {
                itemsDisplayed[key].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");
                if (totalLevel >= key.item.goopLevelCap)
                {
                    Color c = new Color(255, 240, 0, 1.0f);
                    itemsDisplayed[key].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().color = c;
                }
            }
        }
    }

    public InventorySlot findInventorySlotMatch(InventorySlot slot)
    {
        foreach(var item in itemsDisplayed)
        {
            if(item.Key.item.Equals(slot.item))
            {
                return item.Key;
            }    
        }
        return null;
    }


}
