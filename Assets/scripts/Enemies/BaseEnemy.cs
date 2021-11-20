using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseEnemyKnockBackInterface
{

    IEnumerator deathKnockBack(Vector2 direction);
    IEnumerator normalKnockBack(Vector2 direction);
    // void AddTarget(GameObject target);
    // void FireAtTarget();
}
public class BaseEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float damage;
    public float maxSpeed;
    public float acceleration;

    [System.NonSerialized]
    public bool isAlive = true;

    [System.NonSerialized]
    public bool isAbleToMove = true;
    public bool isKnockable = true;

    [System.NonSerialized]
    public bool isGroundBased = true;
    public Vector2[] arraySentryLocations;


    [System.NonSerialized]
    public int activeSentryLocationNumber;

    [System.NonSerialized]
    public Rigidbody2D baseRigidbody2D;

    [System.NonSerialized]
    public Collider2D baseCollider2D;

    [System.NonSerialized]
    public int playerLayer;

    [System.NonSerialized]
    public GameObject targetGameObject = null;


    public virtual void StartAfter() {}
    public virtual void recieveAggroRange(GameObject target){}

    void Start()
    {
        isAlive = true;
        isAbleToMove = true;
        baseRigidbody2D = GetComponent<Rigidbody2D>();
        baseCollider2D = GetComponent<Collider2D>();
        playerLayer = LayerMask.NameToLayer("Player");

        if (arraySentryLocations.Length > 0)
        {
            activeSentryLocationNumber = 0;

        }

        StartAfter();


    }

    

    public void sentryLocationUpdate()
    {
        if (arraySentryLocations.Length == 0)
        {
            return;
        }
        if (activeSentryLocationNumber >= arraySentryLocations.Length)
        {
            activeSentryLocationNumber = 0;
        }

        Vector2 newLocation = arraySentryLocations[activeSentryLocationNumber];

        float step = acceleration * Time.deltaTime;

        Vector2 newPosition = Vector2.MoveTowards((Vector2)this.transform.position, (Vector2)newLocation, step);
        Vector2 newPositionDifference = newPosition - (Vector2)this.transform.position;

        if (isGroundBased)
        {
            newPositionDifference.y = 0f;
        }

        Vector2 newVelocity = baseRigidbody2D.velocity + newPositionDifference;

        if (isGroundBased)
        {
            if (newVelocity.x > maxSpeed)
            {
                newVelocity.x = maxSpeed;
            }
            else if (newVelocity.x < -maxSpeed)
            {
                newVelocity.x = -maxSpeed;
            }
        }
        else
        {
            newVelocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
        }


        if ((transform.position.x + 0.3f) > newLocation.x && (transform.position.x - 0.3f) < newLocation.x)
        {
            newVelocity.x = 0;
        }
        if ((transform.position.y + 0.3f) > newLocation.y && (transform.position.y - 0.3f) < newLocation.y)
        {
            newVelocity.y = 0;
        }

        baseRigidbody2D.velocity = newVelocity;
        if (baseRigidbody2D.velocity.x == 0 && baseRigidbody2D.velocity.y == 0)
        {
            activeSentryLocationNumber++;
        }

        // if (isGroundBased)
        // {
        //     baseRigidbody2D.velocity = new Vector2(baseRigidbody2D.velocity.x, 0f);
        // }

    }

    public virtual IEnumerator normalKnockBack(Vector2 direction)
    {

        baseRigidbody2D.velocity = direction * 20;
        isAbleToMove = false;

        yield return new WaitForSeconds(0.1f);

        baseRigidbody2D.velocity = new Vector2(0f, 0f);
        isAbleToMove = true;
    }

    public virtual IEnumerator deathKnockBack(Vector2 direction)
    {

        baseRigidbody2D.velocity = (direction + new Vector2(0f, 2f)) * 5;
        // baseRigidbody2D.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(2f);
    }


    public virtual void onHit(object[] tempObject)
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
            // Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            // Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), playerCollider, true);
            StartCoroutine(deathKnockBack(directionOfForce));

        }
        else
        {
            if (isKnockable)
            {
                StartCoroutine(normalKnockBack(directionOfForce));
            }

        }
    }

    


}
