using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy ,BaseEnemyKnockBackInterface
{

    public float firstLocalXposition;
    public float secondLocalXposition;
    private bool isGoingToFirst = true;
    // private Rigidbody2D baseRigidbody2D;
    int knockBackMultiplier = 5;
    

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
        if (isGoingToFirst)
        {
            if (transform.localPosition.x < firstLocalXposition)
            {
                baseRigidbody2D.velocity = new Vector2(speed, 0f);
            }
            else
            {
                baseRigidbody2D.velocity = new Vector2(-speed, 0f);
            }

            if ((transform.localPosition.x + 0.3f) > firstLocalXposition && (transform.localPosition.x - 0.3f) < firstLocalXposition)
            {

                isGoingToFirst = false;
            }

        }
        else
        {
            if (transform.localPosition.x < secondLocalXposition)
            {
                baseRigidbody2D.velocity = new Vector2(speed, 0f);
            }
            else
            {
                baseRigidbody2D.velocity = new Vector2(-speed, 0f);
            }
            if ((transform.localPosition.x + 0.3f) > secondLocalXposition && (transform.localPosition.x - 0.3f) < secondLocalXposition)
            {

                isGoingToFirst = true;
            }
        }
    }


    public void onHit(object[] tempObject)
    {
        // public void onHit(float incomingDamage, Vector2 originLocation) {
        float incomingDamage = (float)tempObject[0];

        Vector2 originLocation = (Vector2)tempObject[1];

        health = health - incomingDamage;
        Vector2 knockBackVelocity;

        if (transform.position.x > originLocation.x)
        {
            knockBackVelocity = Vector2.right;
        }
        else
        {
            knockBackVelocity = Vector2.left;
        }




        if (health <= 0)
        {

            isAlive = false;
            Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), playerCollider, true);
            
            StartCoroutine(deathKnockBack());

            // after death animation call baseRigidbody2D.simulated = false;

        }
        else
        {
            StartCoroutine(normalKnockBack(knockBackVelocity));
        }
    }

     public IEnumerator normalKnockBack(Vector2 knockBackVelocity)
    {

        Debug.Log("normalKnockBack inside slime");
        baseRigidbody2D.velocity = knockBackVelocity * knockBackMultiplier;
        isAbleToMove = false;

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
