using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullResults : MonoBehaviour
{
    public List<GoopObject> pullResults;
    public int highestRarity;

    void Start()
    {
        pullResults = new List<GoopObject>();
        highestRarity = 4;
    }
}
