using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    /* old transform stuff
    public int xStart;
    public int yStart;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;
    */

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

            var obj = Instantiate(slot.item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetGoop[slot.item.Id].uiDisplay;
            //obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");

            if (totalLevel >= slot.item.goopLevelCap)
            {
                Color c = new Color(222, 0, 0, 1.0f);
                itemsDisplayed[slot].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().color = c;
            }
        }
    }

    /* Old transform method
    public Vector3 GetPosition(int i)
    {
        return new Vector3((xStart + X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), yStart + (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMN)), 0f);
    }
    */

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {
            InventorySlot slot = inventory.container.Items[i];
            int totalLevel = slot.item.goopLevel + slot.item.goopDuplicates;

            //Adds new object to itemsDisplayed if it does not currently have the object
            if (!itemsDisplayed.ContainsKey(slot))
            {
                var obj = Instantiate(slot.item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetGoop[slot.item.Id].uiDisplay;
                //obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");
                itemsDisplayed.Add(slot, obj);

                if (totalLevel >= slot.item.goopLevelCap)
                {
                    Color c = new Color(222, 0, 0, 1.0f);
                    obj.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().color = c;
                }
            }
            else //If object exists in ItemsDisplayed, update UI 
            {
                itemsDisplayed[slot].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");
                if (totalLevel >= slot.item.goopLevelCap)
                {
                    Color c = new Color(222, 0, 0, 1.0f);
                    itemsDisplayed[slot].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().color = c;
                }
            }
        }
    }
}
