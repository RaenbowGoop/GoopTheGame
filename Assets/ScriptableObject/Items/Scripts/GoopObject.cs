using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
[CreateAssetMenu(fileName = "New Goop Object", menuName = "Inventory System/Items/Goop")]
public class GoopObject : ItemObject, System.IEquatable<GoopObject>
{
    //public Animator goopAnimator;
    [JsonProperty] public string[] goopTags;
    [JsonProperty] public string goopFaction;
    [JsonProperty] public string goopName;
    [JsonProperty] public int goopLevel;
    [JsonProperty] public int goopLevelCap;
    [JsonProperty] public int goopRarity;
    [JsonProperty] public int goopDuplicates; // number of duplicate units aquired. used for bonus levels

    [JsonProperty] public double goopHealth;
    [JsonProperty] public double goopAttack;
    [JsonProperty] public double goopDefense;
    [JsonProperty] public int goopSpeed;
    [JsonProperty] public float goopAttackInterval; //Attack Speed
    [JsonProperty] public float goopCastTime; //Attack Cast Time
    [JsonProperty] public int goopDC;  //Deployment Cost
    [JsonProperty] public float goopCDT; //Cooldown time

    [JsonProperty] public int knockbackLimit;
    

    public void Awake()
    {
        type = ItemType.Goop;
    }

    public void ResetValues()
    {
        goopLevel = 1;
        goopDuplicates = 0;
    }

    public double CalculateHealth()
    {
        return goopHealth * (1 + (goopLevel - 1) * 0.1 + (goopDuplicates * 0.1));
    }

    public double CalculateAttack()
    {
        return goopAttack * (1 + (goopLevel - 1) * 0.1 + (goopDuplicates * 0.1));
    }

    public double CalculateDefense()
    {
        return goopDefense * (1 + (goopLevel - 1) * 0.1 + (goopDuplicates * 0.1));
    }

    public bool Equals(GoopObject other)
    {
        if (other == null)
        {
            return false;
        }
        return this.goopFaction == other.goopFaction && this.goopName == other.goopName;
    }

    public int CompareTo(GoopObject other)
    {
        if (other == null)
        {
            return 1;
        }
        else if (this.goopFaction == other.goopFaction && this.goopName == other.goopName)
        {
            return 0;
        }
        return -1;
    }

    //used to sort GoopObjects
    public int CompareGoopObjectDefault(GoopObject other)
    {
        // A null value means that this object is greater.
        if (other == null)
        {
            return 1;
        } 
        else
        {
            int currentComparison = CompareByLevel(other);
            if (currentComparison == 0)
            {
                currentComparison = CompareByRarity(other);
                if (currentComparison == 0)
                {
                    currentComparison = CompareByDuplicates(other);
                    if (currentComparison == 0)
                    {
                        currentComparison = CompareByFaction(other);
                        if (currentComparison == 0)
                        {
                            return CompareByName(other);
                        }
                        else
                        {
                            return currentComparison;
                        }
                    }
                    else
                    {
                        return currentComparison;
                    }
                }
                else
                {
                    return currentComparison;
                }
            }
            else
            {
                return currentComparison;
            }
        }       
    }

    public int CompareByRarity(GoopObject other)
    {
        //comparing GoopRarity
        if (this.goopRarity == other.goopRarity)
        {
           return 0;
        }
        else if (this.goopRarity > other.goopRarity)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public int CompareByFaction(GoopObject other)
    {
        return this.goopFaction.CompareTo(other.goopFaction);
    }

    public int CompareByLevel(GoopObject other)
    {
        if (this.goopLevel == other.goopLevel)
        {
            return 0;
        }
        else if (this.goopLevel > other.goopLevel)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public int CompareByDuplicates(GoopObject other)
    {
        if (this.goopDuplicates == other.goopDuplicates)
        {
            return 0;
        }
        else if (this.goopDuplicates > other.goopDuplicates)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public int CompareByName(GoopObject other)
    {
        return this.goopName.CompareTo(other.goopName);
    }
}
