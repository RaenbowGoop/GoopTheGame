using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopUnitBehavior : MonoBehaviour
{
    public float startX;
    public GoopObject unit;

    bool isOnCooldown;
    bool isKnockingBack;
    float attackIntervalTimer; //time between attacks
    Animator unitAnimator;

    System.Random randNumGen;
    List<GameObject> currentTargets;

    //unit stats
    public double unitHealth; 
    int knockBackCounter;

    float KBTimer;

    // Start is called before the first frame update
    void Start()
    {
        isOnCooldown = false;
        isKnockingBack = false;
        attackIntervalTimer = 0;

        unitHealth = unit.goopHealth;
        knockBackCounter = unit.goopKnockbackLimit;

        currentTargets = new List<GameObject>();
        randNumGen = new System.Random();
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //if unit is dead and has played death animation, destroy gameobject
        if (unitAnimator.GetBool("isDead") && unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Destroy(this.gameObject);
        }

        //advancing attackIntervalTimer
        if (isOnCooldown && attackIntervalTimer < unit.goopAttackInterval)
        {
            attackIntervalTimer += Time.deltaTime;
        }
        else
        {
            attackIntervalTimer = 0;
            isOnCooldown = false;
        }

        //checking if health checkpoints have been reached
        if (unitHealth <= (1.0 * (knockBackCounter - 1) / unit.goopKnockbackLimit) * unit.goopHealth)
        {
            knockBackCounter--;
            unitHealth = (1.0 * knockBackCounter / unit.goopKnockbackLimit) * unit.goopHealth;
            isKnockingBack = true;
            unitAnimator.Play("Knockback");
        }


        //checking if unit is in process of being knocked back
        if (isKnockingBack)
        {
            transform.position = new Vector2(transform.position.x + Time.deltaTime * 2, transform.position.y); ;
            //if the unit has no more health, declare it DEAD
            if (unitHealth <= 0)
            {
                unitAnimator.SetBool("isDead", true);
            }
            //set isKnockingback to false when unit is done with KB animation
            if (KBTimer >= .5)
            {
                isKnockingBack = false;
                KBTimer = 0;
                if(currentTargets.Count == 0)
                {
                    unitAnimator.SetBool("isAttacking", false);
                    unitAnimator.Play("Idle");
                }
            }
            else
            {
                KBTimer += Time.deltaTime;
            }
        }
        else
        {
            if(!unitAnimator.GetBool("isDead"))
            {
                //check if unit is currently in process of attacking
                if (!unitAnimator.GetBool("isAttacking"))
                {
                    //check if targets are in range
                    if (currentTargets.Count == 0)
                    {
                        transform.position = new Vector2(transform.position.x - Time.deltaTime * unit.goopSpeed, transform.position.y);
                        unitAnimator.SetBool("hasTarget", false);
                    }
                    else
                    {
                        unitAnimator.Play("Idle");
                        if (!isOnCooldown)
                        {
                            unitAnimator.SetBool("hasTarget", true);
                            unitAnimator.SetBool("isAttacking", true);
                        }
                    }
                }
                else
                {
                    if (unitAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base.Reel") && !isOnCooldown)
                    {
                        //play attack impact animations too
                        PerformAttack();
                        isOnCooldown = true;
                        unitAnimator.SetBool("isAttacking", false);
                    }
                    else if (!(unitAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base.Reel")) && isOnCooldown)
                    {
                        unitAnimator.SetBool("isAttacking", false);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "UnitHitbox" && collider.gameObject.tag == "YenUnit")
        {
            currentTargets.Add(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "UnitHitbox" && collider.gameObject.tag == "YenUnit")
        {
            currentTargets.Remove(collider.gameObject);
        }
    }

    void PerformAttack()
    {
        if (unit.goopTargetType == "ST")
        {
            if (currentTargets.Count != 0 && currentTargets[0] != null)
            {
                DealDamage(currentTargets[0]);
            }
        }
        else
        {
            for (int i = 0; i < currentTargets.Count; i++)
            {
                if (currentTargets[i] != null)
                {
                    DealDamage(currentTargets[i]);
                }
            }
        }
    }

    void DealDamage(GameObject target)
    {
        double currentDamage = unit.goopAttack;
        target.transform.parent.parent.GetComponent<YenUnitBehavior>().unitHealth -= currentDamage;
    }
}
