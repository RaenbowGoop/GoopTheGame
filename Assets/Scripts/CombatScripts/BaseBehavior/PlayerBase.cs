using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] InventoryObject lineup;

    public double baseHealth;

    float currentIceCream;
    float iceCreamCap;
    int baseLevel;
    float iceCreamRate; //icecream per second
    float iceCreamTimer;

    List<GameObject> currentDeployedUnits;

    // Start is called before the first frame update
    void Start()
    {
        baseHealth = 10000;
        currentIceCream = 5000;
        baseLevel = 1;
        iceCreamCap = 5000;
        iceCreamTimer = 0f;
        iceCreamRate = 50;

        currentDeployedUnits = new List<GameObject>();
        SetUpLineup();
    }

    // Update is called once per frame
    void Update()
    {
        //incrementing ice cream currency
        if (currentIceCream + Time.deltaTime * iceCreamRate >= iceCreamCap)
        {
            currentIceCream = iceCreamCap;
        }
        else
        {
            currentIceCream += (Time.deltaTime * iceCreamRate);
        }
    }

    public void DeployUnit(CombatUnitSlot unitSlot)
    {
        //check if unit is affordable, not on cooldown, and there are less than 50 units currently on the field
        if(!unitSlot.isOnCooldown && unitSlot.goop.goopDC <= currentIceCream && currentDeployedUnits.Count < 50)
        {
            currentIceCream -= unitSlot.goop.goopDC;
            unitSlot.SetOnCooldown();

            //instantiate prefab
            GameObject obj = Instantiate(unitSlot.goopPrefab, this.transform.position, Quaternion.identity);

            obj.transform.SetParent(this.transform);
            currentDeployedUnits.Add(obj);
        }
        else
        {
            //play sound

        }
    }

    public void RemoveUnit(GameObject obj)
    {
        currentDeployedUnits.Remove(obj);
    }

    void SetUpLineup()
    {
        GameObject selectionPanel = GameObject.FindWithTag("UnitDisplay");
        for (int i = 0; i < lineup.container.Items.Count; i ++)
        {
            GameObject unitSlot = selectionPanel.transform.GetChild(i).gameObject;
            unitSlot.transform.GetChild(0).GetChild(1).GetComponent<CombatUnitSlot>().SetGoop(lineup.container.Items[i].item);

            //set sprites
            unitSlot.transform.GetChild(0).GetChild(1).GetComponent<Image>()
                .sprite = lineup.container.Items[i].item.uiDisplay;

            //setting listener to button
            unitSlot.transform.GetChild(0).GetChild(1).GetComponent<Button>()
                .onClick.AddListener(() => DeployUnit(unitSlot.transform.GetChild(0).GetChild(1).GetComponent<CombatUnitSlot>()));

            
        }
        if(lineup.container.Items.Count > 0 && lineup.container.Items.Count < 8)
        {
            for(int i = lineup.container.Items.Count; i < 8; i++ )
            {
                selectionPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
