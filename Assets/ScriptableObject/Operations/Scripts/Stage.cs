using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Stage", menuName = "Operations/Stage")]
public class Stage : ScriptableObject
{
    //Stage Information
    public string chapterName;
    public int stageNumber;
    public string stageName;
    public int toleranceCost;
    public bool firstClear;

    //Enemy Base Stats
    public double baseHealth;

    //Appearing Enemies
    public YenObject[] featuredUnits;
    public int[] powerMultiplier;
}