using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopManager : MonoBehaviour
{
    public GoopObject currentGoop;

    public void LevelUnit()
    {
        if(currentGoop != null && currentGoop.goopLevel < currentGoop.goopLevelCap)
        {
            currentGoop.goopLevel++;
        }
    }

    public void setCurrentGoop(ref GoopObject goop)
    {
        currentGoop = goop;
    }
}
