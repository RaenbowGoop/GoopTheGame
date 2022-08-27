using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    public double initialBaseHealth;
    public double baseHealth;

    public List<GameObject> currentDeployedUnits;

    public float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentDeployedUnits = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void DeployUnit(YenObject unit, int powerMulitplier)
    {
        if(currentDeployedUnits.Count < 70)
        {
            string unitPrefabPath = "Prefab/YenUnits/" + unit.yenName + "/" + unit.yenName + "Prefab";
            GameObject unitPrefab = Resources.Load<GameObject>(unitPrefabPath);

            //instantiate prefab
            GameObject obj = Instantiate(unitPrefab, this.transform.position, Quaternion.identity);

            //set unit stats
            YenUnitBehavior yenUnitBehaviorScript = obj.transform.GetChild(0).GetComponent<YenUnitBehavior>();
            yenUnitBehaviorScript.SetInitialStats(yenUnitBehaviorScript.unit.yenHealth * powerMulitplier, yenUnitBehaviorScript.unit.yenAttack * powerMulitplier);

            obj.transform.SetParent(this.transform);
            currentDeployedUnits.Add(obj);
        }
    }

    public void RemoveUnit(GameObject obj)
    {
        currentDeployedUnits.Remove(obj);
    }

    public void SetInitialStats(double baseHP)
    {
        //Set base HP
        baseHealth = baseHP;
        initialBaseHealth = baseHP;

        //Display Enemy Base HP
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = initialBaseHealth.ToString();
    }

    public void KillAll()
    {
        for (int i = 0; i < currentDeployedUnits.Count; i++)
        {
            YenUnitBehavior GUBscript = currentDeployedUnits[i].transform.GetChild(0).GetComponent<YenUnitBehavior>();
            GUBscript.knockBackCounter = 1;
            GUBscript.unitHealth = 0;
        }
    }
}
