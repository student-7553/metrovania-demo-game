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
            // if (arraySentryLocations.Length == 1)
            // {
            //     Vector2 newLocation = arraySentryLocations[0];

            //     Vector2 newVelocity = new Vector2(0f, 0f);

            //     float step = acceleration * Time.deltaTime;

            //     Vector2 newPosition = Vector2.MoveTowards((Vector2)this.transform.position, (Vector2)newLocation, step);
            //     Vector2 newPositionDifference = newPosition - (Vector2)this.transform.position;

            //     newVelocity = baseRigidbody2D.velocity + newPositionDifference;
            //     newVelocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
            //     if ((transform.position.x + 0.3f) > newLocation.x && (transform.position.x - 0.3f) < newLocation.x)
            //     {
            //         newVelocity.x = 0;
            //     }
            //     if ((transform.position.y + 0.3f) > newLocation.y && (transform.position.y - 0.3f) < newLocation.y)
            //     {
            //         newVelocity.y = 0;
            //     }
            //     baseRigidbody2D.velocity = newVelocity;

            // }

        }

    }

    public override void StartAfter()
    {
        isGroundBased = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer && targetGameObject == null)
        {
            targetGameObject = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.layer == playerLayer && targetGameObject == null)
        {

            targetGameObject = null;

        }


    }


    public override IEnumerator deathKnockBack(Vector2 direction)
    {
        baseRigidbody2D.velocity = new Vector2(0f, 0f);
        baseRigidbody2D.gravityScale = 3f;
        yield return new WaitForSeconds(2f);
    }
}
