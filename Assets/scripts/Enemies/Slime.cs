using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy
{

    public float firstLocalXposition;
    public float secondLocalXposition;
    
    private bool isGoingToFirst;
    void Start()
    {
        isGoingToFirst = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isGoingToFirst){
            if(transform.localPosition.x < firstLocalXposition){
                transform.localPosition = new Vector2(transform.localPosition.x + (speed * Time.deltaTime), transform.localPosition.y);
            } else {
                transform.localPosition = new Vector2(transform.localPosition.x - (speed * Time.deltaTime), transform.localPosition.y);
            }

            if((transform.localPosition.x + 0.3f) > firstLocalXposition && (transform.localPosition.x - 0.3f) < firstLocalXposition){

                isGoingToFirst = false;
            }   
            
        } else {
             if(transform.localPosition.x < secondLocalXposition){
                transform.localPosition = new Vector2(transform.localPosition.x + (speed * Time.deltaTime), transform.localPosition.y);
            } else {
                transform.localPosition = new Vector2(transform.localPosition.x - (speed * Time.deltaTime), transform.localPosition.y);
            }
            if((transform.localPosition.x + 0.3f) > secondLocalXposition && (transform.localPosition.x - 0.3f) < secondLocalXposition){

                isGoingToFirst = true;
            }  
        }  
    }

    private void onHit(float incomingDamage) {
        Debug.Log(incomingDamage);
    //    gameObject.SetActive(false);

        
    }
}
