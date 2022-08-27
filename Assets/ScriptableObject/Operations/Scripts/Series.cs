using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Series Database", menuName = "Operations/SeriesDatabase")]
public class Series : ScriptableObject
{
    public string seriesName;
    public Chapter[] chapters;
}
