using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YenUnitBehavior : MonoBehaviour
{
    public float startX;
    public YenObject unit;

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

        unitHealth = unit.yenHealth;
        knockBackCounter = unit.yenKnockbackLimit;

        currentTargets = new List<GameObject>();
        randNumGen = new System.Random();
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if unit is dead and has played death animation, destroy gameobject
        if (unitAnimator.GetBool("isDead") && unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(this);
        }

        //checking if health checkpoints have been reached
        if (unitHealth <= (1.0 * (knockBackCounter - 1) / unit.yenKnockbackLimit) * unit.yenHealth)
        {
            knockBackCounter--;
            unitHealth = (1.0 * knockBackCounter / unit.yenKnockbackLimit) * unit.yenHealth;
            isKnockingBack = true;
            unitAnimator.Play("Knockback");
        }

        //checking if unit is in process of being knocked back
        if (isKnockingBack)
        {
            transform.position = new Vector2(transform.position.x - Time.deltaTime * 1, transform.position.y);;
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
                    if (isOnCooldown && attackIntervalTimer < unit.yenAttackInterval)
                    {
                        attackIntervalTimer += Time.deltaTime;
                    }
                    else
                    {
                        attackIntervalTimer = 0;
                        isOnCooldown = false;
                    }
                    //check if targets are in range
                    if (currentTargets.Count == 0 && !isKnockingBack)
                    {
                        transform.position = new Vector2(transform.position.x + Time.deltaTime * unit.yenSpeed, transform.position.y);
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
                        unitAnimator.SetBool("isAttacking", false);
                        isOnCooldown = true;
                    }
                }
            }
        }   
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "UnitHitbox" && collider.gameObject.tag == "GoopUnit")
        {
            currentTargets.Add(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "UnitHitbox" && collider.gameObject.tag == "GoopUnit")
        {
            currentTargets.Remove(collider.gameObject);
        }
    }

    void PerformAttack()
    {
        if (unit.yenTargetType == "ST")
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
                if(currentTargets[i] != null)
                {
                    DealDamage(currentTargets[i]);
                }
            }
        }
    }

    void DealDamage(GameObject target)
    {
        double currentDamage = unit.yenAttack;
        target.transform.parent.parent.GetComponent<GoopUnitBehavior>().unitHealth -= currentDamage;
    }
}
