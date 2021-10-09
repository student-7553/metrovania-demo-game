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

    public bool isJumping;
    public bool isCrouching;

    public bool hasDashed;

    public bool groundTouch;

    public bool isFacingRight;



    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;



    private Rigidbody2D playerRigidBody;
    private PlayerInput playerInput;
    private PlayerCollision playerCollision;
    private AnimationScript animationScript;
    private BoxCollider2D boxCollider;
    public bool betterJumpEnabled = true;
    private float coyoteTime;
    private float currentSpeed;

    private float dashWaitTimer = .3f;

    

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
        boxCollider = GetComponent<BoxCollider2D>();

        currentSpeed = normalSpeed;
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
        if (isJumping && playerRigidBody.velocity.y > 3)
        {
            // corner correction
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
        isJumping = true;
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
        playerRigidBody.velocity += dir * jumpForce;
        animationScript.SetTrigger("jump");

        StartCoroutine(JumpFinishHandler());
        jumpParticle.Play();

    }

    IEnumerator JumpFinishHandler()
    {
        DOVirtual.Float(13, 0, .5f, RigidbodyDrag);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CheckForLanding());
        isJumping = false;
    }


    private void WallJump()
    {

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));
        StartCoroutine(ChangeIsHorizontalLerp(.1f, false));

        Vector2 wallDir = playerCollision.onRightWall ? Vector2.left : Vector2.right;
        wallGrab = false;
        isHorizontalLerp = true;
        // betterJumpEnabled = true;
        Jump((Vector2.up / 3f + wallDir), true);


    }
    private void WallSlide()
    {

        if (!canMove)
        {
            return;
        }

        float newX = playerRigidBody.velocity.x > 0 ? 2 : -2;
        playerRigidBody.velocity = new Vector2(newX, -slideSpeed);

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
        hasDashed = false;
        isDashing = false;

        jumpParticle.Play();
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
        yield return new WaitForSeconds(.15f);
        if (playerCollision.onGround)
        {
            hasDashed = false;
        }

    }

    private void Dash(float x, float y)
    {
        // Camera.main.transform.DOComplete();
        // Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        // FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;
        Vector2 dir = new Vector2(0, 0);
        float midfloat = 0.8f;
        float mid2float = 0.6f;
        float highFloat = 1.2f;

        float value = (float)((Mathf.Atan2(x, y) / Math.PI) * 180f);
        if (value < 0) value += 360f;

        if (value >= 337.5f || value <= 22.5f)
        {
            // up
            dir.x = 0f;
            dir.y = 1f;
        }
        else if (value >= 22.5f && value <= 80f)
        {
            // upright
            dir.x = midfloat;
            dir.y = mid2float;
        }
        else if (value >= 80f && value <= 100)
        {
            // right
            dir.x = highFloat;
            dir.y = 0;
        }
        else if (value >= 100 && value <= 157.5f)
        {
            // rightDown
            dir.x = midfloat;
            dir.y = -mid2float;
        }
        else if (value >= 157.5f && value <= 202.5f)
        {
            // down
            dir.x = 0;
            dir.y = -1f;
        }
        else if (value >= 202.5f && value <= 260)
        {
            // downLeft
            dir.x = -midfloat;
            dir.y = -mid2float;
        }
        else if (value >= 260 && value <= 280)
        {
            // left
            dir.x = -highFloat;
            dir.y = 0;
        }
        else if (value >= 280 && value <= 337.5f)
        {
            // leftUp
            dir.x = -midfloat;
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
        // canMove = false;
        betterJumpEnabled = false;
        isHorizontalLerp = true;
        isDashing = true;
        // Tween mytween = DOVirtual.Float(0, 20, .3f, (float x) =>
        // {
        // });
        // yield return mytween.WaitForCompletion();
    
        yield return DashWaitCounter();

        

        // playerRigidBody.velocity = Vector2.zero;
        playerRigidBody.gravityScale = 3;

        // canMove = true;
        betterJumpEnabled = true;
        isHorizontalLerp = false;
        isDashing = false;

    }

    IEnumerator DashWaitCounter() {

        playerRigidBody.drag = 1;

        yield return new WaitForSeconds(.05f);
        playerRigidBody.drag = 3;

        yield return new WaitForSeconds(.05f);
        playerRigidBody.drag = 9;

        yield return new WaitForSeconds(.05f);
        playerRigidBody.drag = 27;

        yield return new WaitForSeconds(.05f);
        playerRigidBody.drag = 27;

        yield return new WaitForSeconds(.05f);
        playerRigidBody.drag = 81;

        yield return new WaitForSeconds(.01f);
        
    }

    IEnumerator WallClimbUp()
    {

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));
        bool isRight;
        if (playerCollision.onRightBottomWall)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        isHorizontalLerp = true;
        playerRigidBody.velocity += Vector2.up * 10;
        yield return new WaitForSeconds(.07f);
        // playerRigidBody.velocity += Vector2.up * 3;
        // yield return new WaitForSeconds(.03f);
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0f);
        if (isRight)
        {
            playerRigidBody.velocity += Vector2.right * 8;
            // playerRigidBody.velocity = new Vector2(normalSpeed, 0);
        }
        else
        {
            playerRigidBody.velocity += Vector2.left * 8;
            // playerRigidBody.velocity = new Vector2(-normalSpeed, 0);
        }
        yield return new WaitForSeconds(.1f);
        isHorizontalLerp = false;
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
            else
            {
                StartCoroutine(WallClimbUp());

            }



        }
        else
        {
            // betterJumpEnabled = true;
            if (!isDashing)
            {
                playerRigidBody.gravityScale = 3;
            }

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
        if (playerCollision.onWall && !playerCollision.onGround)
        {
            if (x != 0f && !wallGrab)
            {
                if(isJumping){
                    if(playerRigidBody.velocity.y < 3){
                        WallSlide();
                    }
                } else {
                    WallSlide();
                }
                
            }
        }
        if (!playerCollision.onWall || playerCollision.onGround)
        {
            wallSlide = false;
        }
        // END WALL SLIDE
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



        if (playerInput.jumpPressed && !isJumping)
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

        if (playerInput.dashPressed && !hasDashed)
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

        if (isFacingRight == true && x < 0 && playerCollision.onGround)
        {

            animationScript.SetTrigger("flip");
        }
        else if (isFacingRight == false && x > 0 && playerCollision.onGround)
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

        BetterJumping();
        limitVelocityY();


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
        else if (playerRigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
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
