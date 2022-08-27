using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Chapter Database", menuName = "Operations/Chapter Database")]
public class Chapter : ScriptableObject
{
    public string chapterName;
    public Stage[] stages;
}
