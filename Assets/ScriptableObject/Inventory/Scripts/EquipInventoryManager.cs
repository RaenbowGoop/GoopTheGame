using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipInventoryManager : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject lineup;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    int indexOfCurrentGoop = -1;

    //prefabs
    GameObject sixStarPrefab;
    GameObject fiveStarPrefab;
    GameObject fourStarPrefab;

    // Start is called before the first frame update
    void Start()
    {
        sixStarPrefab = Resources.Load<GameObject>("Prefab\\UI\\InventorySlots\\6StarInventoryItem");
        fiveStarPrefab = Resources.Load<GameObject>("Prefab\\UI\\InventorySlots\\5StarInventoryItem");
        fourStarPrefab = Resources.Load<GameObject>("Prefab\\UI\\InventorySlots\\4StarInventoryItem");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {
            InventorySlot slot = inventory.container.Items[i];
            int totalLevel = slot.item.goopLevel;

            InventorySlot key = findInventorySlotMatchinDictionary(slot);
            if (key == null)
            {
                GameObject obj;
                if (slot.item.goopRarity == 6)
                {
                    obj = Instantiate(sixStarPrefab, Vector3.zero, transform.rotation * Quaternion.Euler(0f, 0f, 0f), transform);
                }
                else if (slot.item.goopRarity == 5)
                {
                    obj = Instantiate(fiveStarPrefab, Vector3.zero, transform.rotation * Quaternion.Euler(0f, 0f, 0f), transform);
                }
                else
                {
                    obj = Instantiate(fourStarPrefab, Vector3.zero, transform.rotation * Quaternion.Euler(0f, 0f, 0f), transform);
                }

                obj.transform.localScale = new Vector3(.69f, .69f, .69f);
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

    private InventorySlot findInventorySlotMatchinDictionary(InventorySlot slot)
    {
        foreach (var item in itemsDisplayed)
        {
            if (item.Key.item.Equals(slot.item))
            {
                return item.Key;
            }
        }
        return null;
    }

    private int findInventorySlotIndexInInventory(InventorySlot slot)
    {
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {
            if (slot.item.goopFaction == inventory.container.Items[i].item.goopFaction && slot.item.goopName == inventory.container.Items[i].item.goopName)
            {
                return i;
            }
        }
        return -1;
    }

    public void addUnitToLineup()
    {
        if (lineup.container.Items.Count < 8)
        {
            for (int i = 0; i < lineup.container.Items.Count; i++)
            {
                if (lineup.container.Items[i].item.Equals(inventory.container.Items[indexOfCurrentGoop].item))
                {
                    return;
                }
            }
            lineup.addItem(inventory.database.getMatchingGoop(inventory.container.Items[indexOfCurrentGoop].item));
        }
    }

    public void displayUnit(InventorySlot goopSlot)
    {
        indexOfCurrentGoop = findInventorySlotIndexInInventory(goopSlot);
        GameObject unitDisplay = GameObject.FindWithTag("UnitDisplay");
        GoopObject goop = goopSlot.item;

        if (unitDisplay != null)
        {
            unitDisplay.transform.GetChild(1).GetComponent<Image>().sprite = goop.uiDisplay;
            unitDisplay.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = goop.description;

            //Displaying Faction and Goop Names
            unitDisplay.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = goop.goopFaction;
            unitDisplay.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = goop.goopName;

            //Displaying Level and Stats
            float attackSpeed = 1.0f / (goop.goopAttackInterval + goop.goopCastTime);
            unitDisplay.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text =
                goop.CalculateHealth() + "\n" + goop.CalculateAttack() + "\n" + goop.CalculateDefense() + "\n" + goop.goopSpeed + "\n"
                + attackSpeed.ToString("0.##") + "/s" + "\n" + goop.goopDC + "\n" + goop.goopCDT;

        }
    }
}
