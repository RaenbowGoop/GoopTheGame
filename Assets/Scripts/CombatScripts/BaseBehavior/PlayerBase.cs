using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBase : MonoBehaviour
{
    public double baseHealth;

    [SerializeField] InventoryObject lineup;
    [SerializeField] GameObject GameplayUI;
    Transform icecreamText;
    Transform unitsDeployedNumberText;

    float currentIceCream;
    float iceCreamCap;
    int baseLevel;
    float iceCreamRate; //icecream per second
    float iceCreamTimer;
    bool lineupIsSetUp;

    List<GameObject> currentDeployedUnits;

    // Start is called before the first frame update
    void Start()
    {
        baseHealth = 10000;
        currentIceCream = 1000;
        baseLevel = 1;
        iceCreamCap = 15000;
        iceCreamTimer = 0f;
        iceCreamRate = 100;

        currentDeployedUnits = new List<GameObject>();
        SetUpLineup();

        icecreamText = GameplayUI.transform.GetChild(0).GetChild(0);
        unitsDeployedNumberText = GameplayUI.transform.GetChild(1);

        //Set IceCream Text
        icecreamText.GetChild(0).GetComponent<TextMeshProUGUI>().text = (int)currentIceCream + "/" + iceCreamCap;
        icecreamText.GetChild(1).GetComponent<TextMeshProUGUI>().text = (int)currentIceCream + "/" + iceCreamCap;
    }

    // Update is called once per frame
    void Update()
    {

        if(!lineupIsSetUp)
        {
            SetUpLineup();
            lineupIsSetUp = true;
        }
        
        //incrementing ice cream currency
        if (currentIceCream + Time.deltaTime * iceCreamRate >= iceCreamCap)
        {
            currentIceCream = iceCreamCap;
        }
        else
        {
            addIcecream(Time.deltaTime * iceCreamRate);
            icecreamText.GetChild(0).GetComponent<TextMeshProUGUI>().text = (int)currentIceCream + "/" + iceCreamCap;
            icecreamText.GetChild(1).GetComponent<TextMeshProUGUI>().text = (int)currentIceCream + "/" + iceCreamCap;
        }

        //Update Units Deployed UI
        unitsDeployedNumberText.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentDeployedUnits.Count + "/50";
        unitsDeployedNumberText.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentDeployedUnits.Count + "/50";
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

            obj.transform.GetChild(0).GetComponent<GoopUnitBehavior>().unit = unitSlot.transform.GetComponent<CombatUnitSlot>().goop;

            obj.transform.SetParent(this.transform);
            currentDeployedUnits.Add(obj);

            unitSlot.transform.parent.parent.GetChild(4).GetComponent<AudioSource>().Play();
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
            unitSlot.transform.GetChild(1).GetChild(1).GetComponent<CombatUnitSlot>().SetGoop(lineup.container.Items[i].item);

            //set sprites
            unitSlot.transform.GetChild(1).GetChild(1).GetComponent<Image>()
                .sprite = lineup.container.Items[i].item.uiDisplay;

            //set deployment cost texts
            unitSlot.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                unitSlot.transform.GetChild(1).GetChild(1).GetComponent<CombatUnitSlot>().goop.goopDC.ToString();
            unitSlot.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text =
                unitSlot.transform.GetChild(1).GetChild(1).GetComponent<CombatUnitSlot>().goop.goopDC.ToString();

            //setting listener to button
            unitSlot.transform.GetChild(1).GetChild(1).GetComponent<Button>()
                .onClick.AddListener(() => DeployUnit(unitSlot.transform.GetChild(1).GetChild(1).GetComponent<CombatUnitSlot>()));

            
        }
        if(lineup.container.Items.Count > 0 && lineup.container.Items.Count < 8)
        {
            for(int i = lineup.container.Items.Count; i < 8; i++ )
            {
                selectionPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void addIcecream(float iceCreamDrop)
    {
        currentIceCream += iceCreamDrop;
    }

    public void KillAll()
    {
        for(int i = 0; i < currentDeployedUnits.Count; i++)
        {
            GoopUnitBehavior GUBscript = currentDeployedUnits[i].transform.GetChild(0).GetComponent<GoopUnitBehavior>();
            GUBscript.knockBackCounter = 1;
            GUBscript.unitHealth = 0;
        }
    }
}
