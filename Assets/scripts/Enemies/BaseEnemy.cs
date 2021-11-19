using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseEnemyKnockBackInterface {

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
    public bool isAlive = true;
    public bool isAbleToMove = true;
    public bool isKnockable = true;
  

    [System.NonSerialized]
    public Rigidbody2D baseRigidbody2D;
    [System.NonSerialized]
    public Collider2D baseCollider2D;
    [System.NonSerialized]
    public int playerLayer;

    void Start()
    {
        isAlive = true;
        isAbleToMove = true;
        baseRigidbody2D = GetComponent<Rigidbody2D>();
        baseCollider2D = GetComponent<Collider2D>();
        playerLayer = LayerMask.NameToLayer("Player");
        

    }

    // private void OnCollisionEnter2D(Collision2D collsion) {
    //     if (collsion.gameObject.layer == playerLayer)
    //     {
    //         Debug.Log("collider.gameObject.name/" + collsion.gameObject.name);
    //     }
        
    // }






}
