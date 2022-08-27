using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUnitSlot : MonoBehaviour
{
    public GoopObject goop;
    public GameObject goopPrefab;
    float cooldownTimer;
    public bool isOnCooldown;
    public AudioSource buttonSFX;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTimer = 0f;
        isOnCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnCooldown)
        {
            if (cooldownTimer < goop.goopCDT)
            {
                cooldownTimer += Time.deltaTime;
                this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((int)(goop.goopCDT - cooldownTimer + 1)).ToString();
            }
            else
            {
                cooldownTimer = 0;
                isOnCooldown = false;

                //hide timer and overlay
                this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = false;
                this.transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }  
    }

    public void SetOnCooldown()
    {
        //put unit on cooldown
        isOnCooldown = true;

        //turn on text
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = true;

        //set text to goopCDT
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((int)goop.goopCDT).ToString();

        //turn on dark overlay
        this.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void SetGoop(GoopObject unit)
    {
        goop = unit;
        string goopUnitPrefabPath = "Prefab/GoopUnits/" + goop.goopFaction + "/" + goop.goopName + "Prefab";
        goopPrefab = Resources.Load<GameObject>(goopUnitPrefabPath);
    }
}
