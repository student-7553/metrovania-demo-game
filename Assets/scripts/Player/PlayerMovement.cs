using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{


    [Space]
    [Header("Stats")]


    public float normalSpeed = 10;

    public float currentSpeed = 10;
    public float jumpForce = 20;
    public float slideSpeed = 3;
    public float crouchSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    public float coyoteDuration = .5f;


    [Space]
    [Header("Limit")]
    public float verticalVelocityLimit = 40f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;

    public bool isDashing;

    public bool isJumping;
    public bool isCrouching;

    public bool hasDashed;

    public bool groundTouch;



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

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
        boxCollider = GetComponent<BoxCollider2D>();
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
        if (!wallJumped)
        {
            playerRigidBody.velocity = newVel;
        }
        else
        {

            playerRigidBody.velocity = Vector2.Lerp(playerRigidBody.velocity, newVel, wallJumpLerp * Time.deltaTime);
        }

    }

    private void Jump(Vector2 dir, bool wall)
    {
       
        isJumping = true;
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
        playerRigidBody.velocity += dir * jumpForce;


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

        Vector2 wallDir = playerCollision.onRightWall ? Vector2.left : Vector2.right;
        wallGrab = false;
        wallJumped = true;
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

    private void limitVelocityY()
    {

        // START limit verticalVelocity
        if (playerRigidBody.velocity.y < -verticalVelocityLimit)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, -verticalVelocityLimit);
        }
        else if (playerRigidBody.velocity.y > verticalVelocityLimit)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, verticalVelocityLimit);
        }
        // END limit verticalVelocity
    }

    private void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        jumpParticle.Play();
    }
    void RigidbodyDrag(float x)
    {
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

        animationScript.SetTrigger("dash");

        Vector2 dir = new Vector2(x, y);

        playerRigidBody.velocity = Vector2.zero + (dir.normalized * dashSpeed);
        Debug.Log(playerRigidBody.velocity);

        StartCoroutine(DashWait());
    }
    IEnumerator DashWait()
    {

        // FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        // DOVirtual.Float(14, 0, .8f, RigidbodyDrag);
        // DOVirtual.Float(1, 0, .8f, RigidbodyDrag);
        // dashParticle.Play();
        playerRigidBody.gravityScale = 0;
        // canMove = false;
        betterJumpEnabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.15f);
        playerRigidBody.gravityScale = 3;
        playerRigidBody.velocity = Vector2.zero;
        // canMove = true;
        betterJumpEnabled = true;
        wallJumped = false;
        isDashing = false;

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
        playerRigidBody.velocity += Vector2.up * 4;
        wallJumped = true;

        yield return new WaitForSeconds(.05f);
        playerRigidBody.velocity += Vector2.up * 4;
        yield return new WaitForSeconds(.05f);
        if (isRight)
        {
            playerRigidBody.velocity = new Vector2(normalSpeed, 0);
        }
        else
        {

            playerRigidBody.velocity = new Vector2(-normalSpeed, 0);
        }
        yield return new WaitForSeconds(.1f);
        wallJumped = false;
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


    private void updateWallGrab(float x, float y)
    {
        if (wallGrab && !isDashing)
        {
            betterJumpEnabled = false;
            if (playerCollision.onWall)
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
            betterJumpEnabled = true;
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

        if (!playerInput.grabHeld || !playerCollision.onWall || !canMove || wallJumped)
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
                WallSlide();
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
        Walk(dir);

        if (canMove)
        {
            animationScript.SetHorizontalMovement(x, y, playerRigidBody.velocity.y);
        }



        if (playerCollision.onGround && !isDashing)
        {
            
            coyoteTime = Time.time + coyoteDuration;
            wallJumped = false;
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

        BetterJumping();
        limitVelocityY();

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


}
