using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy, BaseEnemyKnockBackInterface
{

    public float firstLocalXposition;
    public float secondLocalXposition;
    private bool isGoingToFirst = true;
    // private Rigidbody2D baseRigidbody2D;
    int knockBackMultiplier = 20;


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
                baseRigidbody2D.velocity = new Vector2(maxSpeed, 0f);
            }
            else
            {
                baseRigidbody2D.velocity = new Vector2(-maxSpeed, 0f);
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
                baseRigidbody2D.velocity = new Vector2(maxSpeed, 0f);
            }
            else
            {
                baseRigidbody2D.velocity = new Vector2(-maxSpeed, 0f);
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

        Vector2 direction = (Vector2)tempObject[1];

        health = health - incomingDamage;

        if (health <= 0)
        {
            isAlive = false;
            isAbleToMove = false;

            Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), playerCollider, true);
            StartCoroutine(deathKnockBack(direction));
            // after death animation call baseRigidbody2D.simulated = false;
        }
        else
        {
            StartCoroutine(normalKnockBack(direction));
        }
    }

    public IEnumerator normalKnockBack(Vector2 direction)
    {

        baseRigidbody2D.velocity = direction * knockBackMultiplier;
        isAbleToMove = false;

        yield return new WaitForSeconds(0.1f);

        baseRigidbody2D.velocity = new Vector2(0f, 0f);
        isAbleToMove = true;
    }

    public IEnumerator deathKnockBack(Vector2 direction)
    {

        // baseRigidbody2D.velocity = direction * knockBackMultiplier;
        baseRigidbody2D.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(2f);
    }


}
