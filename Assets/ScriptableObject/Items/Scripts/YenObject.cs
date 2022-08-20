using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New Yen Object", menuName = "Inventory System/Items/Yen")]
public class YenObject : ItemObject
{
    public string[] yenTags;
    public string yenName;
    public int yenLevel;
    public int yenTier;

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

    //Status Effects
    public int freezeChance;  //chance to freeze enemy's animation for a period of time
    public int shatterChance; //chance to decrease enemy's attack for a period of time

    //Extra Damage Effects
    public int criticalHitChance; //chance to increase damage dealt to enemy

    public void Awake()
    {
        type = ItemType.Yen;
    }
}
