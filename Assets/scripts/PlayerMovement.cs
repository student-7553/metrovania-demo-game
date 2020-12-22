using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    

    [Space]
    [Header("Stats")]

    
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    
    [Space]
    [Header("Stats")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.5f;

    [Space]
    [Header("Booleans")]
    public bool canMove;

    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;

    public bool isDashing;

    public bool isJumping = false;


    private Rigidbody2D playerRigidBody;
    // private PlayerInput playerInput;
    private PlayerCollision playerCollision;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        // playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
    }

    private void Walk(Vector2 dir){
        if (!canMove){
            return;   
        }
        
        playerRigidBody.velocity = new Vector2(dir.x * speed, playerRigidBody.velocity.y);
        // }
        // else
        // {
        //     rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        // }
    }
    private void Jump(Vector2 dir, bool wall){
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
        playerRigidBody.velocity += dir * jumpForce;
        // isJumping = true;
    }

    

    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        Vector2 dir = new Vector2(x, 0f);
        Walk(dir);


        if (Input.GetButtonDown("Jump") && playerCollision.isOnGround){
            Jump(Vector2.up, false); 
        } 
        else if (isJumping){
            

			// if (jumpTime <= Time.time){
            //     isJumping = false;
            // }
				
        }
        if(playerRigidBody.velocity.y < 0){
            playerRigidBody.velocity += Vector2.up  * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if(playerRigidBody.velocity.y > 0 && !Input.GetButton("Jump")){
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }






        
        
        
        


    }

    
}
