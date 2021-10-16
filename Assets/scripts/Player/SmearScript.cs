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
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        if(move.isFacingRight && !isRight){
            isRight = true;
            anim.SetFloat("isRightFloat", 1);
            transform.localPosition = new Vector2( transform.localPosition.x > 0 ? transform.localPosition.x : -transform.localPosition.x , transform.localPosition.y );
        } else if (!move.isFacingRight && isRight){
            isRight = false;
            anim.SetFloat("isRightFloat", -1);
            transform.localPosition = new Vector2( transform.localPosition.x > 0 ? -transform.localPosition.x : transform.localPosition.x, transform.localPosition.y );
        }
    
        anim.SetFloat("attackCounter", attack.attackAnimationCounter);

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
