using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatInteract : MonoBehaviour
{
    private int playerLayer;	
    private bool isSquished;

    void Start()
    {
        // playerRigidBody = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Player");
        isSquished= false;
    }



    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != playerLayer){
            return;
        }
        if(!isSquished){
            Debug.Log("we are here");
            isSquished = true;
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.125f);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("leaving");
        if(collision.gameObject.layer != playerLayer){
            return;
        }
        Debug.Log("leaving 2");
        if(isSquished){
            isSquished = false;
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.125f);
        }
    }

}
