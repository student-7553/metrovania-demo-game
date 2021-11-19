using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCharger : BaseEnemy, BaseEnemyKnockBackInterface
{
    private GameObject targetGameObject = null;
    void Update()
    {

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
