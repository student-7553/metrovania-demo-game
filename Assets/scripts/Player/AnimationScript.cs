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
    [HideInInspector]
    public SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<PlayerCollision>();
        move = GetComponentInParent<PlayerMovement>();
        attack = GetComponentInParent<PlayerAttack>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        anim.SetBool("onGround", coll.onGround);
        anim.SetBool("onWall", coll.onWall);
        anim.SetBool("onRightWall", coll.onRightWall);
        anim.SetBool("isWallGrabing", move.wallGrab);
        anim.SetBool("isWallSliding", move.wallSlide);
        anim.SetBool("canMove", move.canMove);
        anim.SetBool("isDashing", move.isDashing);
        anim.SetBool("isAttacking", attack.isAttacking);

    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {

        if(x > 0.001){
            anim.SetFloat("isFacingRight", 1);
        } else if (x < -0.001){
            anim.SetFloat("isFacingRight", -1);
        
        }
        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    // public void Flip(int side)
    // {

    //     if (move.wallGrab || move.wallSlide)
    //     {
    //         if (side == -1 && sr.flipX)
    //             return;

    //         if (side == 1 && !sr.flipX)
    //         {
    //             return;
    //         }
    //     }

    //     bool state = (side == 1) ? false : true;
    //     sr.flipX = state;
    // }
}
