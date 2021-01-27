using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    

    [Space]
    [Header("Stats")]

    
    public float normalSpeed = 10;
    public float crouchSpeed = 5;
    public float currentSpeed = 10;
    public float jumpForce = 20;
    public float slideSpeed = 3;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    
    [Space]
    [Header("Stats")]
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 10f;

    [Space]
    [Header("Limit")]
    public float verticalVelocityLimit = 40f;

    [Space]
    [Header("Booleans")]
    public bool canMove;

    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;

    public bool isDashing;

    public bool isJumping;
    public bool isCrouching;

    // public bool isJumping = false;


    private Rigidbody2D playerRigidBody;
    private PlayerInput playerInput;
    private PlayerCollision playerCollision;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
    }

    private void Walk(Vector2 dir){
        if (!canMove){
            return;   
        }
        
        playerRigidBody.velocity = new Vector2(dir.x * currentSpeed, playerRigidBody.velocity.y);
    }

    private void Jump(Vector2 dir, bool wall){
        isJumping = true;
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
        playerRigidBody.velocity += dir * jumpForce;
        StartCoroutine(JumpFinishHandler());

    }

    IEnumerator JumpFinishHandler(){

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CheckForLanding());
        isJumping = false;
    }

    IEnumerator CheckForJumping(){
        while (true)
        {
            if(!playerCollision.onGround){
                break;
            } else {
                yield return new WaitForSeconds(0.1f);
            }
            
        }
        
    }

    IEnumerator CheckForLanding(){
        while (true)
        {
            if(playerCollision.onGround){;
                break;
                
            } else {
                yield return new WaitForSeconds(0.1f);
            }
            
            
        }
    }

    private void WallSlide() {

        if(!canMove){
            return;
        }
        float newX = playerRigidBody.velocity.x > 0 ? 2 : -2;
        playerRigidBody.velocity = new Vector2(newX, -slideSpeed);

    }

    private void Crouch(){
        if(!canMove){
            return;
        }
        isCrouching = true;
        Vector3 scale = transform.localScale;
        
        transform.localScale = new Vector3(scale.x,scale.y / 2 , scale.z); 
        transform.position = new Vector3(transform.position.x,transform.position.y -  (scale.y / 4), transform.position.z); 

        currentSpeed = crouchSpeed;
        
    }
    

    private void stopCrouch(){
        if(!canMove){
            return;
        }
        isCrouching = false;
        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y * 2 , transform.localScale.z); 

        currentSpeed = normalSpeed;
        
    }


    

    
    void Update()
    {
        float playerHorizontalX = Input.GetAxis("Horizontal");
        Vector2 playerHorizontalDirection = new Vector2(playerHorizontalX, 0f);
        Walk(playerHorizontalDirection);

    
        if (playerInput.jumpPressed && playerCollision.onGround && !isJumping){
            Jump(Vector2.up, false); 
        } 

        //slide wall
        if (playerCollision.onWall && !playerCollision.onGround){
            if( playerHorizontalX != 0f && !wallGrab) {
                WallSlide();
            }
        }

        //crouch
        if((playerInput.crouchPressed || playerInput.crouchHeld) && playerCollision.onGround && !isCrouching){
            Crouch();
        }

        if(isCrouching && (!playerInput.crouchHeld )){
            stopCrouch();
        }

        //grab wall

        //dash


        // better jumping
        if(playerRigidBody.velocity.y < 0){
            playerRigidBody.velocity += Vector2.up  * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        // } else if(playerRigidBody.velocity.y > 0 && !Input.GetButton("Jump")){
        } else if(playerRigidBody.velocity.y > 0 && !playerInput.jumpHeld){
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // limit verticalVelocity
        if(playerRigidBody.velocity.y < -verticalVelocityLimit ){
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, -verticalVelocityLimit);
        } else if(playerRigidBody.velocity.y > verticalVelocityLimit){
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, verticalVelocityLimit);
        }






        
        
        
        


    }

    
}
