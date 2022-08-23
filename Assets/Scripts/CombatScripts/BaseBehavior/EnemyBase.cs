using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] YenObject[] featuredUnits;
    [SerializeField] YenObject[] featuredUnitsLevel;

    public double baseHealth;

    List<GameObject> currentDeployedUnits;

    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        baseHealth = 10000;
        currentDeployedUnits = new List<GameObject>();
        //DeployUnit(featuredUnits[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 10)
        {
            timer += Time.deltaTime;
        }
        else
        {
            //DeployUnit(featuredUnits[0]);
            timer = 0f;
        }
    }

    public void DeployUnit(YenObject unit)
    {
        if(currentDeployedUnits.Count < 70)
        {
            string unitPrefabPath = "Prefab/YenUnits/" + unit.yenName + "/" + unit.yenName + "Prefab";
            GameObject unitPrefab = Resources.Load<GameObject>(unitPrefabPath);

            //instantiate prefab
            GameObject obj = Instantiate(unitPrefab, this.transform.position, Quaternion.identity);

            obj.transform.SetParent(this.transform);
            currentDeployedUnits.Add(obj);
        }
    }

    public void RemoveUnit(GameObject obj)
    {
        currentDeployedUnits.Remove(obj);
    }

    public void SetInitialStats(float baseHP)
    {
        baseHealth = baseHP;
    }
}
