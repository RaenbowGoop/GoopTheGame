using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_1_1 : MonoBehaviour, StageDeployer
{
    [SerializeField] EnemyBase enemyBase;
    [SerializeField] Stage stage;

    //Stage Specific
    int ghostBurstCount;

    public string DistributeRewards()
    {
        /*
         * FIRST CLEAR REWARDS:
         *      X150 Goop Bucks
         *      x30000 Goop Potions
         *      
         * REWARDS:
         *      x150 Goop Bucks
         *      x10000 Goop Potions
         */

        //if first clear
            //distribute first clear rewards

        //distribute clear rewards

        string rewardsList = "";
        return rewardsList;
    }

    void Start()
    {
        //Set enemyBase HP
        enemyBase.SetInitialStats(stage.baseHealth);

        //DEPLOY DUMMY UNIT
        enemyBase.DeployUnit(stage.featuredUnits[0], stage.powerMultiplier[0]);

        //Set Stage Specific Variables
        ghostBurstCount = 3;
    }

    void Update()
    {
        // Spawn ghost
        if (enemyBase.timer % 10 <= .003)
        {
            enemyBase.DeployUnit(stage.featuredUnits[0], stage.powerMultiplier[0]);
        }

        // Spawn 3 ghosts at 1/3 enemyBase HP
        if (ghostBurstCount != 0)
        {
            if (enemyBase.baseHealth <= (enemyBase.initialBaseHealth) / 3)
            {
                if (enemyBase.timer % 1 <= .003)
                {
                    enemyBase.DeployUnit(stage.featuredUnits[0], stage.powerMultiplier[0]);
                    ghostBurstCount--;
                }
            }
        }
    }
}
