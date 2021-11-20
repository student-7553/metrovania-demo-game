using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFollower : BaseEnemy, BaseEnemyKnockBackInterface
{
    // Start is called before the first frame update

    // public Vector2[] arraySentryLocations;



    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAlive)
        {
            return;
        }
        if (!isAbleToMove)
        {
            return;
        }

        if (targetGameObject != null)
        {

            float step = acceleration * Time.deltaTime;
            Vector2 newPosition = Vector2.MoveTowards((Vector2)this.transform.position, (Vector2)targetGameObject.transform.position + new Vector2(0f, 2f), step);
            Vector2 newPositionDifference = newPosition - (Vector2)this.transform.position;

            baseRigidbody2D.velocity = baseRigidbody2D.velocity + newPositionDifference;
            baseRigidbody2D.velocity = Vector2.ClampMagnitude(baseRigidbody2D.velocity, maxSpeed);



        }
        else
        {
            this.sentryLocationUpdate();


        }

    }

    public override void StartAfter()
    {
        isGroundBased = false;
    }

    public override void recieveAggroRange(GameObject target)
    {
        if (target.layer == playerLayer && targetGameObject == null)
        {
            targetGameObject = target;
        }
    }


    public override IEnumerator deathKnockBack(Vector2 direction)
    {
        baseRigidbody2D.velocity = new Vector2(0f, 0f);
        baseRigidbody2D.gravityScale = 3f;
        yield return new WaitForSeconds(2f);
    }
}
