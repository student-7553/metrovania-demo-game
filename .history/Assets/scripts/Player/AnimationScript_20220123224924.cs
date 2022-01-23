using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{

    private Animator anim;
    private PlayerMovement move;
    private PlayerCollision coll;

    private PlayerAttack attack;
    // private Rigidbody2D playerRigidBody;

    [HideInInspector]
    // public SpriteRenderer sr;


    

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<PlayerCollision>();
        move = GetComponentInParent<PlayerMovement>();
        attack = GetComponentInParent<PlayerAttack>();
        // sr = GetComponent<SpriteRenderer>();
        // playerRigidBody = GetComponentInParent<Rigidbody2D>();

        anim.SetFloat("isFacingRight", 1);
    }

    void Update()
    {
        // Logic
        if(!anim.GetBool("isWallGrabing") && move.wallGrab){
            anim.SetTrigger("wallGrab");
        }
        
        if(!anim.GetBool("onGround") && coll.onGround){
            anim.SetTrigger("onGroundEntry");
        }

         if(!anim.GetBool("isGettingKnockedBack") && move.isGettingKnockedBack){
            anim.SetTrigger("isGettingKnockedBack");
        }



        anim.SetBool("onGround", coll.onGround);
        
        anim.SetFloat("onGroudFloat", coll.onGround ? 1f : -1f);

        anim.SetBool("onWall", coll.onWall);
        anim.SetBool("onRightWall", coll.onRightWall);
        anim.SetBool("isWallGrabing", move.wallGrab);
        anim.SetBool("isWallSliding", move.wallSlide);
        anim.SetBool("canMove", move.canMove);
        anim.SetBool("isDashing", move.isDashing);
        anim.SetBool("isAttacking", attack.isBasicAttacking);



    }

    public void SetHorizontalMovement(float x,float y, float yVel, float xVel)
    {

        // if(x > 0.001 && isFacingRight == false){
        //     anim.SetFloat("isFacingRight", 1);
        //     isFacingRight = true;
        // } else if (x < -0.001 && isFacingRight == true){
        //     anim.SetFloat("isFacingRight", -1);
        //     isFacingRight = false;
        // }

        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);

        if(yVel > 0.1 || yVel < 0.1){
            anim.SetFloat("VerticalVelocity", yVel);
        } else {
            anim.SetFloat("VerticalVelocity", 0);
        }

        if(xVel > 0.1 || xVel < 0.1){
            anim.SetFloat("HorizontalVelocity", xVel);
        } else {
            anim.SetFloat("HorizontalVelocity", 0);
        }

        if(move.isFacingRight){
            anim.SetFloat("isFacingRight", 1);
        } else {
            anim.SetFloat("isFacingRight", -1 );
        }
        
        
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void SetFloat(string key,  float givenNumber){
        anim.SetFloat(key,givenNumber);
    }



}
