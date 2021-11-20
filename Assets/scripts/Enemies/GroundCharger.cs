using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundCharger : BaseEnemy, BaseEnemyKnockBackInterface
{

    public float aggroBreakDistance = 15f;
    public float chargeAnticipationTime = 1.5f;
    public float chargingVelocity;
    public float chargeDuration;
    public float chargeCooldownTime;
    bool isCharging = false;

    void Update()
    {
        if (targetGameObject != null)
        {
            if (!isCharging)
            {
                if (Vector2.Distance(this.transform.position, targetGameObject.transform.position) > aggroBreakDistance)
                {
                    targetGameObject = null;
                    return;
                }
            }


            if (!isCharging)
            {
                StartCoroutine(ChargeToObject(targetGameObject));
            }

        }
        else
        {
            this.sentryLocationUpdate();

        }
    }


    public IEnumerator ChargeToObject(GameObject targetObject)
    {
        isCharging = true;
        isKnockable = false;
        bool chargeRightDirection;
        if (targetObject.transform.position.x - this.transform.position.x >= 0f)
        {
            chargeRightDirection = true;
        }
        else
        {
            chargeRightDirection = false;
        }


        yield return new WaitForSeconds(chargeAnticipationTime);

        if (!isAlive)
        {
            yield break;
        }

        baseRigidbody2D.gravityScale = 0f;

        baseRigidbody2D.velocity = new Vector2(chargeRightDirection ? chargingVelocity : -chargingVelocity, 0f);


        yield return new WaitForSeconds(chargeDuration);
        if (!isAlive)
        {
            yield break;
        }

        baseRigidbody2D.gravityScale = 3f;
        baseRigidbody2D.drag = 5;
        yield return new WaitForSeconds(0.15f);
        if (!isAlive)
        {
            yield break;
        }
        baseRigidbody2D.drag = 20;
        yield return new WaitForSeconds(0.1f);
        if (!isAlive)
        {
            yield break;
        }
        baseRigidbody2D.drag = 0;
        baseRigidbody2D.velocity = new Vector2(0f, 0f);

        yield return new WaitForSeconds(chargeCooldownTime);
        if (!isAlive)
        {
            yield break;
        }
        isCharging = false;
        isKnockable = true;


    }




    public override void StartAfter()
    {
        isGroundBased = true;
    }

    public override void recieveAggroRange(GameObject target)
    {
        if (target.layer == playerLayer && targetGameObject == null)
        {
            targetGameObject = target;
        }
    }


}
