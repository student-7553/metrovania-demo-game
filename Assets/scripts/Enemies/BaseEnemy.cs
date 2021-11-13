using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float damage;
    public float speed;
    private Rigidbody2D thisRigidBody;


    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHit(float incomingDamage) {

        Debug.Log("are we here?/" + incomingDamage);
        health = health - incomingDamage;
        

        
    }
}
