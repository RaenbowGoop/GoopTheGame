using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New Yen Object", menuName = "Inventory System/Items/Yen")]
public class YenObject : ItemObject
{
    public string[] yenTags;
    public string yenName;
    public int yenTier;
    public float yenIceCreamDrop;

    //Yen's Stats
    public double yenHealth;
    public double yenAttack;
    public int yenSpeed;
    public float yenAttackInterval; //Attack Speed
    public float yenCastTime; //Attack Cast Time
    public float yenRecoveryTime; //Post Attack Recovery Time
    public int yenAttackRange;
    public int yenKnockbackLimit;
    public string yenTargetType; //Single target vs Multi target attacks

    public void Awake()
    {
        type = ItemType.Yen;
    }
}
