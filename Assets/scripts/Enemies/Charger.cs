using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : BaseEnemy, BaseEnemyKnockBackInterface
{
    // Start is called before the first frame update

    public Vector2[] arraySentryLocations;

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
        if (arraySentryLocations.Length == 1)
        {
            Vector2 newLocation = arraySentryLocations[0];

            Vector2 newVelocity = new Vector2(0f, 0f);
            // transform.position;

            if ((transform.position.x + 0.3f) > newLocation.x && (transform.position.x - 0.3f) < newLocation.x)
            {
                // reached x axis 
                newVelocity.x = 0;

            }
            else
            {
                if (newLocation.x > transform.position.x)
                {
                    newVelocity.x = speed;
                }
                else
                {
                    newVelocity.x = -speed;
                }
            }

            if ((transform.position.y + 0.3f) > newLocation.y && (transform.position.y - 0.3f) < newLocation.y)
            {
                newVelocity.y = 0;

            }
            else
            {
                if (newLocation.y > transform.position.y)
                {
                    newVelocity.y = speed;
                }
                else
                {
                    newVelocity.y = -speed;
                }
            }


            baseRigidbody2D.velocity = newVelocity;
        }
    }

    public IEnumerator normalKnockBack(Vector2 knockBackVelocity)
    {

        // Debug.Log("normalKnockBack inside slime");
        // baseRigidbody2D.velocity = knockBackVelocity * knockBackMultiplier;
        // isAbleToMove = false;

        yield return new WaitForSeconds(1f);

        baseRigidbody2D.velocity = new Vector2(0f, 0f);
        isAbleToMove = true;
    }

    public IEnumerator deathKnockBack()
    {
        isAbleToMove = false;
        yield return new WaitForSeconds(2f);
    }
}
