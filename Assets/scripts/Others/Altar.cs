using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    private int playerLayer;	
    private Animator anim;
    private bool highlighted = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != playerLayer){
            return;
        }

        anim.SetBool("highlighted", true);

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer != playerLayer){
            return;
        }
        anim.SetBool("highlighted", false);

    }
}
