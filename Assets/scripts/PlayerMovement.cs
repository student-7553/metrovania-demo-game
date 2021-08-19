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

    public bool hasDashed;

    public bool groundTouch;



    private Rigidbody2D playerRigidBody;
    private PlayerInput playerInput;
    private PlayerCollision playerCollision;
    private AnimationScript animationScript;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
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

    }

    private void WallJump()
    {

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = playerCollision.onRightWall ? Vector2.left : Vector2.right;
        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
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

        transform.localScale = new Vector3(scale.x, scale.y / 2, scale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - (scale.y / 4), transform.position.z);

        currentSpeed = crouchSpeed;

    }


    private void stopCrouch()
    {
        if (!canMove)
        {
            return;
        }
        isCrouching = false;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);

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

        // jumpParticle.Play();
    }
    void RigidbodyDrag(float x)
    {
        playerRigidBody.drag = x;
    }
    IEnumerator DashWait()
    {

        // FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);
        // dashParticle.Play();

        playerRigidBody.gravityScale = 0;
        groundTouch = false;
        // canMove = false;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;
        yield return new WaitForSeconds(.15f);

        // dashParticle.Stop();

        playerRigidBody.gravityScale = 3;
        // canMove = true;
        // isJumping = true;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
        // yield return StartCoroutine(CheckForLanding());
        // isJumping = false;

    }
    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (playerCollision.onGround)
            hasDashed = false;
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

        StartCoroutine(DashWait());
    }



    void Update()
    {


        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        Walk(dir);
        animationScript.SetHorizontalMovement(x, y, playerRigidBody.velocity.y);

        if (playerCollision.onWall && (playerInput.grabPressed || playerInput.grabHeld) && canMove)
        {
            wallGrab = true;
            wallSlide = false;
        }

        if (!playerInput.grabHeld || !playerCollision.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }


        if (playerCollision.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            playerRigidBody.gravityScale = 0;
            if (x > .2f || x < -.2f)
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            }
            float speedModifier = y > 0 ? .5f : 1;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, y * (currentSpeed * speedModifier));
        }
        else
        {
            // if (!isDashing)
            // {
            playerRigidBody.gravityScale = 3;
            // }

        }

        // START WALL SLIDE
        if (playerCollision.onWall && !playerCollision.onGround)
        {
            if (x != 0f && !wallGrab)
            {
                WallSlide();
            }
        }
        if (!playerCollision.onWall || playerCollision.onGround){
             wallSlide = false;
        }
           

        // END WALL SLIDE

        // START CROUCH
        if ((playerInput.crouchPressed || playerInput.crouchHeld) && playerCollision.onGround && !isCrouching)
        {
            Crouch();
        }

        if (isCrouching && (!playerInput.crouchHeld))
        {
            stopCrouch();
        }

         // END CROUCH






        if (playerInput.jumpPressed && !isJumping)
        {
            if (playerCollision.onGround)
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
                // Debug.Log("we are dashing");
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


        // limitVelocityY();














    }


    IEnumerator JumpFinishHandler()
    {

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CheckForLanding());
        isJumping = false;
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
