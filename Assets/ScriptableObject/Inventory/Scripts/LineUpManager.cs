using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LineUpManager : MonoBehaviour
{
    public InventoryObject lineup;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    //prefabs
    GameObject sixStarPrefab;
    GameObject fiveStarPrefab;
    GameObject fourStarPrefab;

    // Start is called before the first frame update
    void Start()
    {
        sixStarPrefab = Resources.Load<GameObject>("Prefab\\UI\\EquipSlots\\6StarEquipItem");
        fiveStarPrefab = Resources.Load<GameObject>("Prefab\\UI\\EquipSlots\\5StarEquipItem");
        fourStarPrefab = Resources.Load<GameObject>("Prefab\\UI\\EquipSlots\\4StarEquipItem");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if(lineup.container.Items.Count != itemsDisplayed.Count)
        {
            resetDisplay();
        }

        for (int i = 0; i < lineup.container.Items.Count; i++)
        {
            InventorySlot slot = lineup.container.Items[i];
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
                obj.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = slot.item.goopDC.ToString("n0");
                obj.transform.GetChild(1).GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() => removeUnit(slot)); 

                itemsDisplayed.Add(slot, obj);
            }
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

    public void removeUnit(InventorySlot slot)
    {
        Destroy(itemsDisplayed[slot]);
        itemsDisplayed.Remove(slot);
        lineup.removeItem(slot);

        //reset Display for sorting
        resetDisplay();
    }

    private void resetDisplay()
    {
        foreach (var item in itemsDisplayed.Values)
        {
            Destroy(item);
        }
        itemsDisplayed.Clear();
    }
}
