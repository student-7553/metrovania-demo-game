using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCharger : BaseEnemy, BaseEnemyKnockBackInterface
{
    public Vector2[] arraySentryLocations;
    private GameObject targetGameObject = null;
    void Update()
    {
        if (targetGameObject != null)
        {

            // float step = acceleration * Time.deltaTime;
            // Vector2 newPosition = Vector2.MoveTowards((Vector2)this.transform.position, (Vector2)targetGameObject.transform.position + new Vector2(0f, 2f), step);
            // Vector2 newPositionDifference = newPosition - (Vector2)this.transform.position;

            // baseRigidbody2D.velocity = baseRigidbody2D.velocity + newPositionDifference;
            // baseRigidbody2D.velocity = Vector2.ClampMagnitude(baseRigidbody2D.velocity, maxSpeed);



        }
        else
        {
            if (arraySentryLocations.Length == 1)
            {
                Vector2 newLocation = arraySentryLocations[0];

                Vector2 newVelocity = new Vector2(0f, 0f);

                float step = acceleration * Time.deltaTime;

                Vector2 newPosition = Vector2.MoveTowards((Vector2)this.transform.position, (Vector2)newLocation, step);
                Vector2 newPositionDifference = newPosition - (Vector2)this.transform.position;

                newVelocity = baseRigidbody2D.velocity + newPositionDifference;
                newVelocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
                if ((transform.position.x + 0.3f) > newLocation.x && (transform.position.x - 0.3f) < newLocation.x)
                {
                    newVelocity.x = 0;
                }
                if ((transform.position.y + 0.3f) > newLocation.y && (transform.position.y - 0.3f) < newLocation.y)
                {
                    newVelocity.y = 0;
                }
                baseRigidbody2D.velocity = newVelocity;

            }

        }
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

    public IEnumerator normalKnockBack(Vector2 direction)
    {

        baseRigidbody2D.velocity = direction * 20;
        isAbleToMove = false;

        yield return new WaitForSeconds(0.1f);

        baseRigidbody2D.velocity = new Vector2(0f, 0f);
        isAbleToMove = true;
    }

    public IEnumerator deathKnockBack(Vector2 direction)
    {

        baseRigidbody2D.velocity = (direction + new Vector2(0f, 2f)) * 5;
        // baseRigidbody2D.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(2f);
    }


    public void onHit(object[] tempObject)
    {

        if (!isAlive)
        {
            return;
        }

        float incomingDamage = (float)tempObject[0];

        Vector2 directionOfForce = (Vector2)tempObject[1];

        health = health - incomingDamage;

        if (health <= 0)
        {

            isAlive = false;
            Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), playerCollider, true);

            StartCoroutine(deathKnockBack(directionOfForce));

        }
        else
        {
            StartCoroutine(normalKnockBack(directionOfForce));
        }
    }
}
