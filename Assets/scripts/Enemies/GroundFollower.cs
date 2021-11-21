using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFollower : BaseEnemy
{
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
            Vector2 newPosition = Vector2.MoveTowards((Vector2)this.transform.position, new Vector2(targetGameObject.transform.position.x, this.transform.position.y), step);
            Vector2 newPositionDifference = newPosition - (Vector2)this.transform.position;

            Vector2 newVelocity = baseRigidbody2D.velocity + newPositionDifference;

            if (newVelocity.x > maxSpeed)
            {
                newVelocity.x = maxSpeed;
            }
            else if (newVelocity.x < -maxSpeed)
            {
                newVelocity.x = -maxSpeed;
            }
            baseRigidbody2D.velocity = newVelocity;

        }
        else
        {
            this.sentryLocationUpdate();


        }

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
