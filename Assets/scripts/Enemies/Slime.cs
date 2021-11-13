using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy
{

    public float firstLocalXposition;
    public float secondLocalXposition;
    private bool isGoingToFirst = true;
    private Rigidbody2D thisRigidBody;
    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isGoingToFirst){
            if(transform.localPosition.x < firstLocalXposition){
                thisRigidBody.velocity = new Vector2(speed,0f);
            } else {
                thisRigidBody.velocity = new Vector2(-speed,0f);
            }

            if((transform.localPosition.x + 0.3f) > firstLocalXposition && (transform.localPosition.x - 0.3f) < firstLocalXposition){

                isGoingToFirst = false;
            }   
            
        } else {
             if(transform.localPosition.x < secondLocalXposition){
                 thisRigidBody.velocity = new Vector2(speed,0f);
            } else {
                thisRigidBody.velocity = new Vector2(-speed,0f);
            }
            if((transform.localPosition.x + 0.3f) > secondLocalXposition && (transform.localPosition.x - 0.3f) < secondLocalXposition){

                isGoingToFirst = true;
            }  
        }  
    }

    // public void onHit(float incomingDamage) {
    //     Debug.Log("are we here SLime inside?" + incomingDamage);
    // //    gameObject.SetActive(false);

        
    // }

    
}
