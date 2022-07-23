using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public InventoryObject inventory;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    int indexOfCurrentGoop = -1;

    // Start is called before the first frame update
    void Start()
    {
        //CreateDisplay();
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        GameObject.FindWithTag("GoopCurrency").GetComponent<TextMeshProUGUI>().text = player.getGoopPotions().ToString("n0");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    /*
    public void CreateDisplay()
    {
        Debug.Log(inventory.container.Items.Count);
        //Creates GoopObjects, sets UI values, and Adds GoopObjects to ItemsDisplayed
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {
            InventorySlot slot = inventory.container.Items[i];
            int totalLevel = slot.item.goopLevel;

            //Instantiating Goop Object
            GameObject obj = Instantiate(slot.item.prefab, Vector3.zero, Quaternion.identity, transform);
            //Setting Ui Display/Sprite
            obj.transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = slot.item.uiDisplay;
            //Setting level
            obj.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");
            //Setting additional level
            if(slot.item.goopDuplicates > 0)
            {
                obj.transform.GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = slot.item.goopDuplicates.ToString("n0");
            }
            else
            {
                obj.transform.GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = "";
                obj.transform.GetChild(7).GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            //Setting up Prefab button to display unit when clicked
            obj.transform.GetChild(1).GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() => displayUnit(slot));
            itemsDisplayed.Add(slot, obj);

            //Changing Level color if level exceeds level cap
            if (totalLevel >= slot.item.goopLevelCap)
            {
                Color c = new Color(255, 222, 0, 1.0f);
                itemsDisplayed[slot].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().color = c;
                itemsDisplayed[slot].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            }
        }

        if(inventory.container.Items.Count > 0)
        {
            indexOfCurrentGoop = 0;
            displayUnit(inventory.container.Items[indexOfCurrentGoop]);
        }
    }
    */

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {
            
            InventorySlot slot = inventory.container.Items[i];
            int totalLevel = slot.item.goopLevel;

            InventorySlot key = findInventorySlotMatchinDictionary(slot);
            if (key == null)
            {
                GameObject obj = Instantiate(slot.item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = slot.item.uiDisplay;
                obj.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");
                obj.transform.GetChild(1).GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() => displayUnit(slot));

                if (totalLevel >= slot.item.goopLevelCap)
                {
                    Color c = new Color(222, 0, 0, 1.0f);
                    obj.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
                    obj.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().color = c;
                }

                if (slot.item.goopDuplicates > 0)
                {
                    obj.transform.GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = slot.item.goopDuplicates.ToString("n0");
                }
                else
                {
                    obj.transform.GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = "";
                    obj.transform.GetChild(7).GetComponentInChildren<TextMeshProUGUI>().text = "";
                }

                itemsDisplayed.Add(slot, obj);
            }
            else //If object exists in ItemsDisplayed, update UI 
            {
                itemsDisplayed[key].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = totalLevel.ToString("n0");
                if (totalLevel >= key.item.goopLevelCap)
                {
                    Color c = new Color(255, 240, 0, 1.0f);
                    itemsDisplayed[key].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
                    itemsDisplayed[key].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().color = c;
                }
            }
        }

        if (indexOfCurrentGoop == -1 && inventory.container.Items.Count > 0)
        {
            indexOfCurrentGoop = 0;
            displayUnit(inventory.container.Items[indexOfCurrentGoop]);
        }
    }

    public InventorySlot findInventorySlotMatchinDictionary(InventorySlot slot)
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

    public int findInventorySlotIndexInInventory(InventorySlot slot)
    {
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {
            if(slot.item.goopFaction == inventory.container.Items[i].item.goopFaction && slot.item.goopName == inventory.container.Items[i].item.goopName)
            {
                return i;
            }
        }
        return -1;
    }

    private void displayUnit(InventorySlot goopSlot)
    {
        indexOfCurrentGoop = findInventorySlotIndexInInventory(goopSlot);
        GameObject unitAndStarDisplay = GameObject.FindWithTag("UnitDisplay");
        GoopObject goop = inventory.container.Items[indexOfCurrentGoop].item;

        //Displaying Unit Art
        unitAndStarDisplay.transform.GetChild(0).GetComponent<Image>().sprite = goop.uiDisplay;

        //Displaying Unit Rarity/Stars
        string starPath = "GoopGraphics\\Stars\\4StarScaled";
        if (goop.goopRarity == 6)
        {
            starPath = "GoopGraphics\\Stars\\6StarScaled";
        }
        else if (goop.goopRarity == 5)
        {
            starPath = "GoopGraphics\\Stars\\5StarScaled";
        }
        unitAndStarDisplay.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(starPath);

        //Displaying Faction and Goop Names
        unitAndStarDisplay.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = goop.goopFaction;
        unitAndStarDisplay.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = goop.goopName;

        //Displaying Level and Stats
        float attackSpeed = 1.0f / (goop.goopAttackInterval + goop.goopCastTime);
        unitAndStarDisplay.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = goop.goopLevel + "/" + goop.goopLevelCap;
        unitAndStarDisplay.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = 
            goop.CalculateHealth() + "\n" + goop.CalculateAttack() + "\n" + goop.CalculateDefense() + "\n" + goop.goopSpeed + "\n"
            + attackSpeed.ToString("0.##") + "/s" + "\n" + goop.goopDC + "\n" + goop.goopCDT;

        //Displaying Extra Levels
        if (goop.goopDuplicates > 0)
        {
            unitAndStarDisplay.transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>().text = "+";
            unitAndStarDisplay.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = goop.goopDuplicates.ToString();
        }
        else
        {
            unitAndStarDisplay.transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
            unitAndStarDisplay.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void LevelUnit()
    {
        if (indexOfCurrentGoop != -1 && inventory.container.Items[indexOfCurrentGoop].item.goopLevel < inventory.container.Items[indexOfCurrentGoop].item.goopLevelCap)
        {
            inventory.container.Items[indexOfCurrentGoop].item.goopLevel++;
            inventory.Save();
            displayUnit(inventory.container.Items[indexOfCurrentGoop]);
        }
    }
}
