using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmearScript : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement move;
    private PlayerAttack attack;

    private bool counter = false;
    private bool isRight = true; 

    void Start()
    {
        anim = GetComponent<Animator>();
        attack = GetComponentInParent<PlayerAttack>();
        move = GetComponentInParent<PlayerMovement>();
    }

    private void handlePosition(Vector3 position, bool isRight){
        if(isRight){
            transform.position += new Vector3( 2,0,0 );
        } else {
            transform.position += new Vector3( -2,0,0 );
        }
        
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        if(x > 0.001 && !isRight){
            isRight = true;
            anim.SetFloat("isRightFloat", 1);
            handlePosition(transform.position,isRight);
        } else if (x < -0.001 && isRight){
            isRight = false;
            anim.SetFloat("isRightFloat", -1);
            handlePosition(transform.position,isRight);

        }
        if(attack.isAttacking){
            if(!counter){
                counter = true;
                anim.SetTrigger("triggered");
            }
        } else {
            if(counter){
                counter = false;
                // anim.SetTrigger("triggered");
            }
        }
        

    }

}
