using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{


    [Space]
    [Header("Stats")]


    public float normalSpeed;
    public float jumpForce;
    public float slideSpeed;
    public float crouchSpeed;
    public float wallJumpLerp;
    public float dashSpeed;
    public float coyoteDuration;


    [Space]
    [Header("Limit")]
    public float verticalVelocityLimit;

    public float fallMultiplier;
    public float lowJumpMultiplier;
    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool isHorizontalLerp;
    public bool wallSlide;

    public bool isDashing;

    public bool isTailEndDashing;

    public bool isJumping;
    public bool isCrouching;

    public int allowedDashes;

    public bool groundTouch;

    public bool isFacingRight;



    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem[] groundEntryParticles;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;



    private Rigidbody2D playerRigidBody;
    private PlayerInput playerInput;
    private PlayerCollision playerCollision;
    private AnimationScript animationScript;
    private BoxCollider2D boxCollider;
    private bool betterJumpEnabled = true;
    private float coyoteTime;
    private float currentSpeed;
    private bool dashFixed = false;
    private bool dashFixRightSide = false;
    private bool jumpFixCoroutineRunning = false;
    public int remainingDashes;


    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
        boxCollider = GetComponent<BoxCollider2D>();

        currentSpeed = normalSpeed;
        remainingDashes = allowedDashes;


    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
        {
            return;
        }

        if (wallGrab)
        {
            return;
        }

        Vector2 newVel = new Vector2(dir.x * currentSpeed, playerRigidBody.velocity.y);

        if(playerCollision.onWall){
            if( !playerCollision.onLeftWall && playerCollision.onLeftTopWall){
                newVel.x = 0f;
            } else if ( !playerCollision.onRightWall  && playerCollision.onRightTopWall) {
                newVel.x = 0f;
            }
        } 

        if (isJumping && playerRigidBody.velocity.y > 3)
        {
            // corner correction while jumping
            if (playerCollision.onWall)
            {
                newVel.x = 0f;
            }
            // else {
            //     playerRigidBody.velocity = newVel;
            // }
            playerRigidBody.velocity = newVel;



        }
        else
        {

            if (!isHorizontalLerp)
            {
                playerRigidBody.velocity = newVel;
            }
            else
            {

                playerRigidBody.velocity = Vector2.Lerp(playerRigidBody.velocity, newVel, wallJumpLerp * Time.deltaTime);
            }

        }


    }

    private void Jump(Vector2 dir, bool wall)
    {
        if (!canMove)
        {
            return;
        }
        isJumping = true;
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
        playerRigidBody.velocity += dir * jumpForce;
        animationScript.SetTrigger("jump");

        StartCoroutine(JumpFinishHandler());

        int particleSide = playerCollision.onRightWall ? 1 : -1;
        slideParticle.transform.parent.localScale = new Vector3(particleSide, 1, 1);
        
        if(wall){
            int particleRotation = playerCollision.onRightWall ? 90 : -90;
            var em = wallJumpParticle.shape;
            em.rotation = new Vector3(em.rotation.x, em.rotation.y, particleRotation);
            wallJumpParticle.Play();
        } else {
            jumpParticle.Play();
        }
        

    }

    IEnumerator JumpFinishHandler()
    {
        DOVirtual.Float(13, 0, .6f, RigidbodyDrag);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CheckForLanding());
        isJumping = false;
    }


    private void WallJump()
    {

       
        StartCoroutine(ChangeIsHorizontalLerp(.1f, false));

        Vector2 wallDir = playerCollision.onRightWall ? Vector2.left : Vector2.right;
        wallGrab = false;
        isHorizontalLerp = true;
        // betterJumpEnabled = true;
        Jump((Vector2.up / 3f + wallDir), true);
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));


    }
    private void WallSlide()
    {

        if (!canMove)
        {
            return;
        }

        float newX = playerRigidBody.velocity.x > 0 ? 2 : -2;
        playerRigidBody.velocity = new Vector2(newX, -slideSpeed);
        wallSlide = true;
        // animationScript.SetTrigger("slide");

    }

    private void Crouch()
    {
        if (!canMove)
        {
            return;
        }
        isCrouching = true;
        Vector3 scale = transform.localScale;
        boxCollider.offset = new Vector2(boxCollider.offset.x, -(boxCollider.size.y / 4));
        boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y / 2);


        currentSpeed = crouchSpeed;

    }
    private void stopCrouch()
    {
        if (!canMove)
        {
            return;
        }
        isCrouching = false;
        boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y * 2);
        boxCollider.offset = new Vector2(boxCollider.offset.x, 0);

        currentSpeed = normalSpeed;

    }



    private void GroundTouch()
    {
        // hasDashed = false;
        remainingDashes = allowedDashes;
        isDashing = false;

        // jumpParticle.Play();
        foreach(var single in groundEntryParticles){
            single.Play();
        }
    }
    void RigidbodyDrag(float x)
    {
        if (isDashing)
        {
            return;
        }
        playerRigidBody.drag = x;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.4f);
        if (playerCollision.onGround)
        {
            // hasDashed = false;
            remainingDashes = allowedDashes;
        }

    }

    private void Dash(float x, float y)
    {
        // Camera.main.transform.DOComplete();
        // Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        // FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        // hasDashed = true;
        remainingDashes--;
        Vector2 dir = new Vector2(0, 0);
        float midfloat = 0.8f;
        float mid2float = 0.6f;
        // float mid3float = 0.4f;
        // float highFloat = 1.2f;

        float value = (float)((Mathf.Atan2(x, y) / Math.PI) * 180f);
        if (value < 0) value += 360f;

        if (value >= 337.5f || value <= 22.5f)
        {
            // up
            dir.x = 0f;
            dir.y = midfloat;
        }
        else if (value >= 22.5f && value <= 80f)
        {
            // upright
            dir.x = mid2float;
            dir.y = mid2float;
        }
        else if (value >= 80f && value <= 100)
        {
            // right
            dir.x = midfloat;
            dir.y = 0;
        }
        else if (value >= 100 && value <= 157.5f)
        {
            // rightDown
            dir.x = mid2float;
            dir.y = -mid2float;
        }
        else if (value >= 157.5f && value <= 202.5f)
        {
            // down
            dir.x = 0;
            dir.y = -midfloat;
        }
        else if (value >= 202.5f && value <= 260)
        {
            // downLeft
            dir.x = -mid2float;
            dir.y = -mid2float;
        }
        else if (value >= 260 && value <= 280)
        {
            // left
            dir.x = -midfloat;
            dir.y = 0;
        }
        else if (value >= 280 && value <= 337.5f)
        {
            // leftUp
            dir.x = -mid2float;
            dir.y = mid2float;
        }

        animationScript.SetTrigger("dash");

        playerRigidBody.velocity = dir * dashSpeed;

        StartCoroutine(DashWait());
    }
    IEnumerator DashWait()
    {

        // FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());

        // dashParticle.Play();
        playerRigidBody.gravityScale = 0;
        canMove = false;
        betterJumpEnabled = false;
        isHorizontalLerp = true;

        isDashing = true;
        isTailEndDashing = true;

        // Tween mytween = DOVirtual.Float(0, 20, .3f, (float x) =>
        // {
        // });
        // yield return mytween.WaitForCompletion();

        yield return DashWaitCounter();
        // yield return DashVelWaitCounter();



        // playerRigidBody.gravityScale = 3;
        canMove = true;
        isHorizontalLerp = false;
        isDashing = false;
        playerRigidBody.gravityScale = 2f;

        yield return new WaitForSeconds(.1f);
        playerRigidBody.gravityScale = 3;
        isTailEndDashing = false;
        // isDashing = false;
        // playerRigidBody.gravityScale = 3;
        
        
        

    }
    // IEnumerator DashVelWaitCounter()
    // {

    //     playerRigidBody.drag = 0;
    //     yield return new WaitForSeconds(.1f);


    // }

    IEnumerator DashWaitCounter()
    {

        playerRigidBody.drag = 0;

        yield return new WaitForSeconds(.1f);
        playerRigidBody.drag = 3;

        yield return new WaitForSeconds(.07f);
        playerRigidBody.drag = 9;

        yield return new WaitForSeconds(.07f);
        playerRigidBody.drag = 27;

        yield return new WaitForSeconds(.02f);

        playerRigidBody.drag = 0;
        
    }


    IEnumerator WallClimbUp()
    {
        bool isRight;
        if (playerCollision.onRightBottomWall)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }

        playerRigidBody.gravityScale = 0;
        betterJumpEnabled = false;
        canMove = false;
        isHorizontalLerp = true;
        playerRigidBody.velocity = Vector2.zero;
        playerRigidBody.velocity += Vector2.up * 15;
        yield return new WaitForSeconds(.07f);
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0f);
        if (isRight)
        {
            playerRigidBody.velocity += Vector2.right * 8;
        }
        else
        {
            playerRigidBody.velocity += Vector2.left * 8;
        }
        yield return new WaitForSeconds(.1f);
        isHorizontalLerp = false;
        canMove = true;
        playerRigidBody.gravityScale = 3;
        betterJumpEnabled = true;
    }




    private void updateWallGrab(float x, float y)
    {
        if (wallGrab && !isDashing)
        {
            betterJumpEnabled = false;
            if (playerCollision.onRightWall || playerCollision.onLeftWall)
            {
                playerRigidBody.gravityScale = 0;
                float speedModifier = y > 0 ? .5f : 1;
                playerRigidBody.velocity = new Vector2(0, y * (currentSpeed * speedModifier));

            }
            else if (playerCollision.onRightBottomWall || playerCollision.onLeftBottomWall)
            {
                StartCoroutine(WallClimbUp());

            }



        }

        if (!wallGrab && !wallSlide && !isTailEndDashing)
        {
            betterJumpEnabled = true;
            playerRigidBody.gravityScale = 3;
        }



        if (playerCollision.onWall && (playerInput.grabPressed || playerInput.grabHeld) && canMove)
        {
            wallGrab = true;
            wallSlide = false;


        }


        if (!playerInput.grabHeld || !playerCollision.onWall || !canMove || isHorizontalLerp)
        {
            wallGrab = false;
            wallSlide = false;
        }

    }

    private void updateWallSlide(float x, float y)
    {

        // START WALL SLIDE
        if (playerCollision.onWall && !playerCollision.onGround && !isDashing)
        {
            // if user is moving towards wall
            if (x != 0f && !wallGrab)
            {
                if (!playerCollision.onRightWall && !playerCollision.onLeftWall && (playerCollision.onRightBottomWall || playerCollision.onLeftBottomWall) && isJumping && !isHorizontalLerp )
                {
                    if(playerRigidBody.velocity.y > 0 && playerRigidBody.velocity.y < 5){
                        StartCoroutine(WallClimbUp());
                    } else if (playerRigidBody.velocity.y < 0) {
                         WallSlide();
                    }

                    
                } else {
                    if ((playerCollision.onRightWall && x >= 1f) || (playerCollision.onLeftWall && x <= -1f))
                    {
                        if (isJumping)
                        {
                            if (playerRigidBody.velocity.y < 3)
                            {
                                WallSlide();
                            }
                        }
                        else
                        {
                            WallSlide();
                        }

                    }
                }

                // check if x is directed towards the correct wall

            }
        }
        if (!playerCollision.onWall || playerCollision.onGround)
        {
            wallSlide = false;
        }
        // END WALL SLIDE
    }

    IEnumerator JumpFix()
    {
        jumpFixCoroutineRunning = true;
        while(playerCollision.onLeftBottomWall || playerCollision.onRightBottomWall){
            playerRigidBody.velocity += Vector2.up * 20;
            yield return null;
        }
        // playerRigidBody.velocity += Vector2.up * 20;
        // yield return new WaitForSeconds(.03f);
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0f);
        jumpFixCoroutineRunning = false;

    }

    private void updateCrouch(float x, float y)
    {
        // START CROUCH


        if (isCrouching && (!playerInput.crouchHeld))
        {
            stopCrouch();
        }
        if ((playerInput.crouchPressed || playerInput.crouchHeld) && playerCollision.onGround && !isCrouching)
        {
            Crouch();
        }

        // END CROUCH
    }
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        if (canMove)
        {
            animationScript.SetHorizontalMovement(x, y, playerRigidBody.velocity.y, playerRigidBody.velocity.x);
        }

        Walk(dir);

        if (playerCollision.onGround && !isDashing)
        {

            coyoteTime = Time.time + coyoteDuration;
            isHorizontalLerp = false;
            betterJumpEnabled = true;
        }

        updateWallGrab(x, y);

        updateWallSlide(x, y);



        if (playerInput.jumpPressed && !isJumping && !isDashing)
        {
            if (playerCollision.onGround || (coyoteTime > Time.time))
            {
                Jump(Vector2.up, false);
            }
            else if (playerCollision.onWall && !playerCollision.onGround)
            {
                WallJump();
            }
        }
        //  if (playerInput.dashPressed)
        if (playerInput.dashPressed && remainingDashes > 0 && !isDashing)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                Dash(xRaw, yRaw);
            }
        }

        if (playerCollision.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }
        if (!playerCollision.onGround && groundTouch)
        {
            groundTouch = false;
        }

        updateCrouch(x, y);

        if (isFacingRight == true && x < 0 && playerCollision.onGround && canMove)
        {

            animationScript.SetTrigger("flip");
        }
        else if (isFacingRight == false && x > 0 && playerCollision.onGround && canMove)
        {

            animationScript.SetTrigger("flip");
        }

        if (playerRigidBody.velocity.x > 0.01 && isFacingRight == false)
        {
            isFacingRight = true;
        }
        else if (playerRigidBody.velocity.x < -0.01 && isFacingRight == true)
        {
            isFacingRight = false;
        }

        WallParticle(y);
        DashFix();
        BetterJumping();
        limitVelocityY();


    }

    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            int particleSide = playerCollision.onRightWall ? 1 : -1;
            slideParticle.transform.parent.localScale = new Vector3(particleSide, 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    private void DashFix()
    {
        if (!isDashing)
        {
            dashFixed = false;
            return;
        }
        if (dashFixed && !playerCollision.onRightBottomWall && !playerCollision.onLeftBottomWall)
        {
            dashFixed = false;
            if(dashFixRightSide){
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.y , playerRigidBody.velocity.y);
            } else  {
                playerRigidBody.velocity = new Vector2(-playerRigidBody.velocity.y , playerRigidBody.velocity.y);
            }
            
        }
        if (playerRigidBody.velocity.x > 0.1f && playerCollision.onRightBottomWall)
        {
            dashFixRightSide = true;
            // right dash
            playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
            dashFixed = true;

        }
        else if (playerRigidBody.velocity.x < 0.1f && playerCollision.onLeftBottomWall)
        {
            dashFixRightSide = false;
            // left dash
            playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
            dashFixed = true;
        }

    }


    private void BetterJumping()
    {
        if (!betterJumpEnabled)
        {
            return;
        }
        if (playerRigidBody.velocity.y < 0)
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRigidBody.velocity.y > 0 && !playerInput.jumpHeld)
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void limitVelocityY()
    {

        // START limit verticalVelocity
        if (playerRigidBody.velocity.y < -verticalVelocityLimit)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, -verticalVelocityLimit);
        }
        // else if (playerRigidBody.velocity.y > verticalVelocityLimit)
        // {
        //     playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, verticalVelocityLimit);
        // }
        // END limit verticalVelocity
    }



    IEnumerator CheckForLanding()
    {
        while (true)
        {
            if (playerCollision.onGround || wallGrab || wallSlide)
            {
                break;

            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }


        }
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator ChangeIsHorizontalLerp(float time, bool state)
    {
        yield return new WaitForSeconds(time);
        isHorizontalLerp = state;
    }

    IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }


}
