using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Goop Object", menuName = "Inventory System/Items/Goop")]

public class GoopObject : ItemObject
{
    public string goopFaction;
    public string goopName;
    public int goopLevel;
    public int goopLevelCap;
    public int goopRarity;

    public int goopHealth;
    public int goopAttack;
    public int goopDefense;

    public float goopAttackInterval; //Attack Speed
    public float goopCastTime; //Attack Cast Time

    public int goopDC;  //Deployment Cost
    public float goopCDT; //Cooldown time

    public void Awake()
    {
        type = ItemType.Goop;
    }
}
