using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoopCollectionManager : MonoBehaviour
{
    public InventoryObject inventory;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    int indexOfCurrentGoop = -1;

    GameObject sixStarPrefab;
    GameObject fiveStarPrefab;
    GameObject fourStarPrefab;

    GameObject currentUnitObj;

    // Start is called before the first frame update
    void Start()
    {
        //Loading in prefabs
        sixStarPrefab = Resources.Load<GameObject>("Prefab/UI/InventorySlots/6StarInventoryItem");
        fiveStarPrefab = Resources.Load<GameObject>("Prefab/UI/InventorySlots/5StarInventoryItem");
        fourStarPrefab = Resources.Load<GameObject>("Prefab/UI/InventorySlots/4StarInventoryItem");

        //Loading Goop Currency
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        GameObject goopPotionGO = GameObject.FindWithTag("GoopCurrency");
        if (goopPotionGO != null)
        {
            goopPotionGO.GetComponent<TextMeshProUGUI>().text = player.getGoopPotions().ToString("n0");
        }

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
                    obj = Instantiate(sixStarPrefab, Vector3.zero, Quaternion.identity, transform);
                }
                else if (slot.item.goopRarity == 5)
                {
                    obj = Instantiate(fiveStarPrefab, Vector3.zero, Quaternion.identity, transform);
                }
                else
                {
                    obj = Instantiate(fourStarPrefab, Vector3.zero, Quaternion.identity, transform);
                }

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
        foreach(var item in itemsDisplayed)
        {
            if(item.Key.item.Equals(slot.item))
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
        GameObject unitDisplay = GameObject.FindWithTag("UnitDisplay");
        GoopObject goop = inventory.container.Items[indexOfCurrentGoop].item;

        if(unitDisplay != null)
        {
            //Displaying Unit Art
            string goopUnitPrefabPath = "Prefab/GoopUnits/" + goop.goopFaction + "/" + goop.goopName + "Prefab";
            GameObject goopPrefab = Resources.Load<GameObject>(goopUnitPrefabPath);

            if(currentUnitObj != null)
            {
                //replaces unit if the selected unit is not currently being displayed
                GoopUnitBehavior gubScript = currentUnitObj.transform.GetChild(0).GetComponent<GoopUnitBehavior>();
                if (!gubScript.unit.Equals(goop))
                {
                    Destroy(currentUnitObj);
                    currentUnitObj = Instantiate(goopPrefab, unitDisplay.transform.position, Quaternion.identity);
                    gubScript = currentUnitObj.transform.GetChild(0).GetComponent<GoopUnitBehavior>();
                    gubScript.SetDisplayMode(true);
                    currentUnitObj.transform.SetParent(unitDisplay.transform.GetChild(0));
                }
            }
            else
            {
                //creates obj with unit prefab if there is no obj being displayed currently
                currentUnitObj = Instantiate(goopPrefab, unitDisplay.transform.position, Quaternion.identity);
                currentUnitObj.transform.GetChild(0).GetComponent<GoopUnitBehavior>().SetDisplayMode(true);
                currentUnitObj.transform.SetParent(unitDisplay.transform.GetChild(0));
            }

            //Displaying Unit Rarity/Stars
            string starPath = "GoopGraphics/Stars/4StarScaled";
            if (goop.goopRarity == 6)
            {
                starPath = "GoopGraphics/Stars/6StarScaled";
            }
            else if (goop.goopRarity == 5)
            {
                starPath = "GoopGraphics/Stars/5StarScaled";
            }
            unitDisplay.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(starPath);

            //Displaying Faction and Goop Names
            unitDisplay.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = goop.goopFaction;
            unitDisplay.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = goop.goopName;

            //Displaying Level and Stats
            float attackSpeed = 1.0f / (goop.goopAttackInterval + goop.goopCastTime);
            unitDisplay.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = goop.goopLevel + "/" + goop.goopLevelCap;
            unitDisplay.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text =
                goop.CalculateHealth() + "\n" + goop.CalculateAttack() + "\n" + goop.CalculateDefense() + "\n" + goop.goopSpeed + "\n"
                + attackSpeed.ToString("0.##") + "/s" + "\n" + goop.goopDC + "\n" + goop.goopCDT;

            //Displaying Extra Levels
            if (goop.goopDuplicates > 0)
            {
                unitDisplay.transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>().text = "+";
                unitDisplay.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = goop.goopDuplicates.ToString();
            }
            else
            {
                unitDisplay.transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
                unitDisplay.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            }
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
