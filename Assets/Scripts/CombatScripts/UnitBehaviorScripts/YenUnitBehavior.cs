using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YenUnitBehavior : MonoBehaviour
{
    public YenObject unit;

    bool isOnCooldown;
    bool isKnockingBack;
    float attackIntervalTimer; //time between attacks
    Animator unitAnimator;

    System.Random randNumGen;
    List<GameObject> currentTargets;

    //unit stats
    public double unitHealth;
    public double initialUnitHealth;
    public double unitAttack;
    public int knockBackCounter;

    float KBTimer;
    const float KB_SPEED = 2.3f;

    public GameObject unitBase;

    // Start is called before the first frame update
    void Start()
    {
        unitBase = GameObject.FindWithTag("EnemyBase");
        isOnCooldown = false;
        isKnockingBack = false;
        attackIntervalTimer = 0;

        knockBackCounter = unit.yenKnockbackLimit;

        currentTargets = new List<GameObject>();
        randNumGen = new System.Random();
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();

        transform.parent.position = new Vector2(unitBase.transform.parent.position.x, unitBase.transform.parent.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy GameObject if unit is declared DEAD
        DestroyObject();

        //advancing attackIntervalTimer
        AdvanceCooldownTimer();

        //checking if health checkpoints have been reached
        CheckHealth();

        //checking if unit is in process of being knocked back
        if (isKnockingBack)
        {
            KnockbackUnit();
        }
        else
        {
            if (!unitAnimator.GetBool("isDead"))
            {
                //check if unit is currently in process of attacking
                if (!unitAnimator.GetBool("isAttacking"))
                {
                    DetermineAction();
                }
                else
                {
                    //Checking if unit is done casting attack
                    if (unitAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base.Reel") && !isOnCooldown)
                    {
                        //play attack impact animations too
                        PerformAttack();
                        isOnCooldown = true;
                       // unitAnimator.SetBool("isAttacking", false);
                    }
                    else if (unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && isOnCooldown)
                    {
                        unitAnimator.SetBool("isAttacking", false);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.name == "UnitHitbox" || collider.gameObject.name == "BaseHitbox") && 
            (collider.gameObject.tag == "GoopUnit"|| collider.gameObject.tag == "PlayerBase"))
        {
            currentTargets.Add(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.gameObject.name == "UnitHitbox" || collider.gameObject.name == "BaseHitbox") &&
            (collider.gameObject.tag == "GoopUnit" || collider.gameObject.tag == "PlayerBase"))
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
                DealDamageToUnit(currentTargets[0]);
            }
        }
        else
        {
            for (int i = 0; i < currentTargets.Count; i++)
            {
                if (currentTargets[i] != null)
                {
                    DealDamageToUnit(currentTargets[i]);
                }
            }
        }
    }

    void DealDamageToUnit(GameObject target)
    {
        //calculate damage
        double currentDamage = unitAttack;
        if (target.tag == "PlayerBase")
        {
            target.transform.parent.GetComponent<PlayerBase>().baseHealth -= currentDamage;
        }
        else
        {
            //perform ailments
            target.transform.parent.parent.GetComponent<GoopUnitBehavior>().unitHealth -= currentDamage;
        }
    }

    void DestroyObject()
    {
        //if unit is dead and has played death animation, destroy gameobject
        if (unitAnimator.GetBool("isDead") && unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            // Distribute Ice Cream Drop
            GameObject.FindWithTag("PlayerBase").transform.parent.GetComponent<PlayerBase>().addIcecream(unit.yenIceCreamDrop);
            this.transform.parent.parent.GetComponent<EnemyBase>().RemoveUnit(this.transform.parent.gameObject);
            this.transform.GetChild(0).GetChild(0).GetComponent<Collider2D>().enabled = false;
            this.transform.GetChild(0).GetChild(1).GetComponent<Collider2D>().enabled = false;
            Destroy(this.transform.parent.gameObject);
        }
    }

    void AdvanceCooldownTimer()
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
    }

    void CheckHealth()
    {
        if (unitHealth <= (1.0 * (knockBackCounter - 1) / unit.yenKnockbackLimit) * initialUnitHealth)
        {
            knockBackCounter--;
            unitHealth = (1.0 * knockBackCounter / unit.yenKnockbackLimit) * initialUnitHealth;
            isKnockingBack = true;
            unitAnimator.Play("Knockback");
        }
    }

    void KnockbackUnit()
    {
        //Move Unit in opposite direction
        transform.position = new Vector2(transform.position.x - Time.deltaTime * KB_SPEED, transform.position.y);

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
            //Set flags if conditions before and after KB are not the same
            if (currentTargets.Count == 0)
            {
                unitAnimator.SetBool("isAttacking", false);
                unitAnimator.Play("Idle");
            }
        }
        else
        {
            //advance timer
            KBTimer += Time.deltaTime;
        }
    }

    void DetermineAction()
    {
        //check if targets are in range
        if (currentTargets.Count == 0)
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

    public void SetInitialStats(double health, double attack)
    {
        initialUnitHealth = health; 
        unitHealth = health;
        unitAttack = attack;
    }
}
