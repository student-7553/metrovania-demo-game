using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFollower : BaseEnemy, BaseEnemyKnockBackInterface
{
    // Start is called before the first frame update

    public Vector2[] arraySentryLocations;
    int knockBackMultiplier = 20;

    private GameObject targetGameObject = null;


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
            Vector2 newPosition = Vector2.MoveTowards((Vector2)this.transform.position, (Vector2)targetGameObject.transform.position + new Vector2(0f,2f), step);
            Vector2 newPositionDifference = newPosition - (Vector2)this.transform.position;

            // Vector2 newPositionDifference = ((Vector2)targetGameObject.transform.position + new Vector2(0f,1f)) - ((Vector2)this.transform.position );
            Debug.Log("newPositionDifference/" + newPositionDifference);
            
            baseRigidbody2D.velocity = baseRigidbody2D.velocity + newPositionDifference;
            baseRigidbody2D.velocity = Vector2.ClampMagnitude(baseRigidbody2D.velocity, maxSpeed);
            // baseRigidbody2D.velocity = newPositionDifference;

            // if (baseRigidbody2D.velocity.x > maxSpeed)
            // {
            //     baseRigidbody2D.velocity = new Vector2(maxSpeed, baseRigidbody2D.velocity.y);
            // }
            // else if (baseRigidbody2D.velocity.x < -maxSpeed)
            // {
            //     baseRigidbody2D.velocity = new Vector2(-maxSpeed, baseRigidbody2D.velocity.y);
            // }

            // if (baseRigidbody2D.velocity.y > maxSpeed)
            // {
            //     baseRigidbody2D.velocity = new Vector2(baseRigidbody2D.velocity.x, maxSpeed);
            // }
            // else if (baseRigidbody2D.velocity.x < -maxSpeed)
            // {
            //     baseRigidbody2D.velocity = new Vector2(baseRigidbody2D.velocity.x, -maxSpeed);
            // }

            



        }
        else
        {
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
                        newVelocity.x = maxSpeed;
                    }
                    else
                    {
                        newVelocity.x = -maxSpeed;
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
                        newVelocity.y = maxSpeed;
                    }
                    else
                    {
                        newVelocity.y = -maxSpeed;
                    }
                }


                baseRigidbody2D.velocity = newVelocity;
            }

        }

    }

    public void onHit(object[] tempObject)
    {
        float incomingDamage = (float)tempObject[0];

        Vector2 directionOfForce = (Vector2)tempObject[1];

        health = health - incomingDamage;
        // Vector2 knockBackVelocity;

        // if (transform.position.x > originLocation.x)
        // {
        //     knockBackVelocity = Vector2.right;
        // }
        // else
        // {
        //     knockBackVelocity = Vector2.left;
        // }
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

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == playerLayer && targetGameObject == null)
        {

            targetGameObject = collision.gameObject;



        }


    }


    public IEnumerator normalKnockBack(Vector2 direction)
    {

        // Debug.Log("normalKnockBack inside slime");
        baseRigidbody2D.velocity = direction * knockBackMultiplier;
        isAbleToMove = false;

        yield return new WaitForSeconds(0.1f);

        baseRigidbody2D.velocity = new Vector2(0f, 0f);
        isAbleToMove = true;
    }

    public IEnumerator deathKnockBack(Vector2 direction)
    {
        baseRigidbody2D.velocity = new Vector2(0f,0f);
        baseRigidbody2D.gravityScale = 3f;
        yield return new WaitForSeconds(2f);
    }
}
