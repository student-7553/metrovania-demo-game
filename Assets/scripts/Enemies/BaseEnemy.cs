using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseEnemyKnockBackInterface {

        IEnumerator deathKnockBack();
        IEnumerator normalKnockBack(Vector2 knockBackVelocity);
        // void AddTarget(GameObject target);
        // void FireAtTarget();
}
public class BaseEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float damage;
    public float speed;
    public bool isAlive;
    public bool isAbleToMove;

    [System.NonSerialized]
    public Rigidbody2D baseRigidbody2D;
    [System.NonSerialized]
    public Collider2D baseCollider2D;

    void Start()
    {
        isAlive = true;
        isAbleToMove = true;
        baseRigidbody2D = GetComponent<Rigidbody2D>();
        baseCollider2D = GetComponent<Collider2D>();

    }





}
