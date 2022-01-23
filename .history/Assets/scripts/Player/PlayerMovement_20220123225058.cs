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
    public float dashLength;
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
    public bool overrideBetterJumping = true;
    public bool isJumping;
    public bool isFacingRight;
    public bool isCrouching;

    public int allowedDashes;
    public int remainingDashes;

    public bool groundTouch;


 




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
    // private BoxCollider2D boxCollider;
    private PlayerAttack playerAttack;
    private bool betterJumpEnabled;
    private float coyoteTime;
    private float currentSpeed;


    private int enemyLayer;
    private int playerLayer;

    private IEnumerator dashCoroutine;


    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
        playerAttack = GetComponent<PlayerAttack>();
        // boxCollider = GetComponent<BoxCollider2D>();

        currentSpeed = normalSpeed;
        remainingDashes = allowedDashes;
        enemyLayer = LayerMask.NameToLayer("Enemies");
        playerLayer = LayerMask.NameToLayer("Player");


        // betterJumpEnabled = true;


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

        if (playerCollision.onWall)
        {
            if (!playerCollision.onLeftWall && playerCollision.onLeftTopWall)
            {
                newVel.x = 0f;
            }
            else if (!playerCollision.onRightWall && playerCollision.onRightTopWall)
            {
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

        if (wall)
        {
            int particleRotation = playerCollision.onRightWall ? 90 : -90;
            var em = wallJumpParticle.shape;
            em.rotation = new Vector3(em.rotation.x, em.rotation.y, particleRotation);
            wallJumpParticle.Play();
        }
        else
        {
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

    }



    private void GroundTouch()
    {
        // hasDashed = false;
        remainingDashes = allowedDashes;
        isDashing = false;

        // jumpParticle.Play();
        foreach (var single in groundEntryParticles)
        {
            single.Play();
        }
    }
    void RigidbodyDrag(float x)
    {
        if (isDashing)
        {
            return;
        }
        if (playerRigidBody)
        {
            playerRigidBody.drag = x;
        }

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
        if (!PlayerData.playerBoolUpgrades.isDashAvailable)
        {
            return;
        }

        remainingDashes--;




        float radian = Mathf.Atan2(x, y);


        float degree = radian * (180 / Mathf.PI);
        if (degree < 0)
        {
            degree = 360f + degree;
        }
        float breakingAngle = 360f / 8f;
        float breakingAngleDiv = breakingAngle / 2f;

        float newDegree = (int)(degree / breakingAngleDiv);
        // float remainder = (degree % breakingAngle);


        if (newDegree % 2 == 0)
        {
            newDegree = (newDegree * breakingAngleDiv);
        }
        else
        {
            newDegree = ((newDegree + 1) * breakingAngleDiv);
        }

        float newRadians = newDegree * ((float)Math.PI / 180f);

        float newX = 1f * Mathf.Sin(newRadians);
        float newY = 1f * Mathf.Cos(newRadians);
        // float newX = 1f * Mathf.Sin(Mathf.Atan2(x, y));
        // float newY = 1f * Mathf.Cos(Mathf.Atan2(x, y));
        Vector2 dir = new Vector2(newX, newY);

        animationScript.SetTrigger("dash");

        bool focused = false;

        if (playerInput.focusHeld && PlayerData.playerFloatResources.currentMana >= PlayerData.playerResourceUsage.focusDash)
        {
            if (PlayerData.playerBoolUpgrades.isSpiritDashAvailable)
            {
                PlayerData.playerFloatResources.currentMana = PlayerData.playerFloatResources.currentMana - PlayerData.playerResourceUsage.focusDash;

                playerCollision.allowEnemyTrigger = false;
                focused = true;
            }


        }


        // playerRigidBody.velocity = dir * tempDashSpeed;

        dashCoroutine = DashAfter(focused, dir);

        StartCoroutine(dashCoroutine);
    }
    IEnumerator DashAfter(bool focused, Vector2 direction)
    {

        StartCoroutine(GroundDash());

        playerRigidBody.gravityScale = 0;
        playerRigidBody.drag = 0;
        playerRigidBody.velocity = new Vector2(0f, 0f);
        canMove = false;

        betterJumpEnabled = false;
        overrideBetterJumping = true;

        isDashing = true;


        yield return DashWaitCounter(focused, direction);


        canMove = true;
        isDashing = false;
        playerRigidBody.gravityScale = 1f;

        yield return new WaitForSeconds(.1f);

        playerRigidBody.gravityScale = 3;
        overrideBetterJumping = false;
        if (focused)
        {
            // Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
            playerCollision.allowEnemyTrigger = true;
        }
    }

    IEnumerator DashWaitCounter(bool focused, Vector2 direction)
    {

        if (focused)
        {
            Vector2 prePosition = transform.position;

            playerRigidBody.drag = 0;

            // yield return new WaitForSeconds(.03f);
            // playerRigidBody.drag = 9;
            // yield return new WaitForSeconds(.05f);

            yield return new WaitForSeconds(.06f);

            playerRigidBody.drag = 42;

            Vector2 postPosition = transform.position;
            playerAttack.DashAttack(prePosition, postPosition);

            yield return new WaitForSeconds(.02f);

            playerRigidBody.velocity = new Vector2(0f, 0f);
            playerRigidBody.drag = 0;



            // yield return new WaitForSeconds(.03f);

            // playerRigidBody.velocity = playerRigidBody.velocity - (playerRigidBody.velocity / 2);

            // yield return new WaitForSeconds(.05f);

            // playerRigidBody.velocity = playerRigidBody.velocity / 4;

            // Vector2 postPosition = transform.position;
            // playerAttack.DashAttack(prePosition, postPosition);

            // playerRigidBody.drag = 22;

            // yield return new WaitForSeconds(.02f);

            // playerRigidBody.drag = 0;
        }
        else
        {

            Vector2 targetPostion = (Vector2)this.transform.position + (direction * dashLength);

            bool dashFixed = false;
            bool dashFixedSecond = false;
            bool dashFixRightSide = false;
            float timer = 0f;


            // secondary timer that if reaches 0 breaks out of this
            while (true)
            {
                timer = timer + Time.deltaTime;
                if (timer > 0.26f)
                {
                    break;
                }
                float step = dashSpeed * Time.deltaTime;
                Vector2 newPosition = Vector2.MoveTowards((Vector2)this.transform.position, targetPostion, step);


                if (dashFixed && !dashFixedSecond && !playerCollision.onRightBottomWall && !playerCollision.onLeftBottomWall)
                {
                    dashFixedSecond = true;
                    if (dashFixRightSide)
                    {

                        targetPostion.x = this.transform.position.x + (targetPostion.y - this.transform.position.y);
                    }
                    else
                    {
                        targetPostion.x = this.transform.position.x - (targetPostion.y - this.transform.position.y);

                    }

                }

                if (!dashFixed && newPosition.x > transform.position.x && playerCollision.onRightBottomWall)
                {
                    dashFixRightSide = true;
                    dashFixed = true;
                    targetPostion.x = this.transform.position.x;

                }
                else if (!dashFixed && newPosition.x < transform.position.x && playerCollision.onLeftBottomWall)
                {
                    dashFixRightSide = false;
                    dashFixed = true;
                    targetPostion.x = this.transform.position.x;
                }

                playerRigidBody.MovePosition(newPosition);

                if (
                    (transform.position.x + 0.1f > targetPostion.x && (transform.position.x - 0.1f) < targetPostion.x) &&
                    (transform.position.y + 0.1f > targetPostion.y && (transform.position.y - 0.1f) < targetPostion.y)
                    )
                {
                    // 
                    break;
                }

                yield return null;



            }

            Debug.Log("timer/" + timer);
        }


    }

    public void DashEscape()
    {
        StopCoroutine(dashCoroutine);
        // StopAllCoroutines();
        canMove = true;
        isDashing = false;
        playerRigidBody.gravityScale = 3;
        overrideBetterJumping = false;

    }

    public void PlayerUpBump()
    {

        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 30f);
        StartCoroutine(DisableBetterJumpSpace(0.4f));
        DOVirtual.Float(10, 0, .5f, RigidbodyDrag);
        // yield return null;
    }

    public IEnumerator SpiritRangedAttackShift(Vector2 direction)
    {
        canMove = false;
        playerRigidBody.gravityScale = 0;
        betterJumpEnabled = false;
        overrideBetterJumping = true;

        playerRigidBody.velocity = (-1 * direction) * 30f;

        yield return new WaitForSeconds(0.05f);
        playerRigidBody.velocity = new Vector2(0f, 0f);


        yield return new WaitForSeconds(0.3f);
        canMove = true;
        playerRigidBody.gravityScale = 3;
        overrideBetterJumping = false;

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
        // isHorizontalLerp = true;
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
        // isHorizontalLerp = false;
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

        if (!wallGrab && !wallSlide && !overrideBetterJumping)
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
                if (!playerCollision.onRightWall && !playerCollision.onLeftWall && (playerCollision.onRightBottomWall || playerCollision.onLeftBottomWall) && isJumping && !isHorizontalLerp)
                {
                    if (playerRigidBody.velocity.y > 0 && playerRigidBody.velocity.y < 5)
                    {
                        StartCoroutine(WallClimbUp());
                    }
                    else if (playerRigidBody.velocity.y < 0)
                    {
                        WallSlide();
                    }


                }
                else
                {
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
        while (playerCollision.onLeftBottomWall || playerCollision.onRightBottomWall)
        {
            playerRigidBody.velocity += Vector2.up * 20;
            yield return null;
        }
        // playerRigidBody.velocity += Vector2.up * 20;
        // yield return new WaitForSeconds(.03f);
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0f);
    }


    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");


        // Vector2 dir = new Vector2(xRaw, yRaw);
        if (canMove)
        {
            animationScript.SetHorizontalMovement(x, y, playerRigidBody.velocity.y, playerRigidBody.velocity.x);
        }
        Vector2 walkDir = new Vector2(x, y);

        Walk(walkDir);

        if (playerCollision.onGround && !isDashing)
        {

            coyoteTime = Time.time + coyoteDuration;
            isHorizontalLerp = false;

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
        // DashFix();
        BetterJumping();
        limitDownwardYVelocity();


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

    // private void DashFix()
    // {
    //     if (!isDashing)
    //     {
    //         dashFixed = false;
    //         return;
    //     }
    //     if (dashFixed && !playerCollision.onRightBottomWall && !playerCollision.onLeftBottomWall)
    //     {
    //         dashFixed = false;
    //         if (dashFixRightSide)
    //         {
    //             playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.y, playerRigidBody.velocity.y);
    //         }
    //         else
    //         {
    //             playerRigidBody.velocity = new Vector2(-playerRigidBody.velocity.y, playerRigidBody.velocity.y);
    //         }

    //     }
    //     if (playerRigidBody.velocity.x > 0.1f && playerCollision.onRightBottomWall)
    //     {
    //         dashFixRightSide = true;
    //         // right dash
    //         playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
    //         dashFixed = true;

    //     }
    //     else if (playerRigidBody.velocity.x < 0.1f && playerCollision.onLeftBottomWall)
    //     {
    //         dashFixRightSide = false;
    //         // left dash
    //         playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
    //         dashFixed = true;
    //     }

    // }


    private void BetterJumping()
    {
        if (!betterJumpEnabled)
        {
            return;
        }
        if (playerRigidBody.velocity.y < -0.3f)
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRigidBody.velocity.y > 0.3f && !playerInput.jumpHeld)
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void limitDownwardYVelocity()
    {

        // START limit verticalVelocity
        if (isDashing)
        {
            return;
        }
        if (playerRigidBody.velocity.y < -verticalVelocityLimit)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, -verticalVelocityLimit);
        }

        // END limit verticalVelocity
    }

    public IEnumerator knockBackPlayer(Vector2 direction, float impactValue)
    {
        animationScript.SetTrigger("isGettingKnockedBack");
        canMove = false;
        isHorizontalLerp = true;
        playerRigidBody.velocity = (direction + new Vector2(0f, 2f)) * impactValue;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        isHorizontalLerp = false;
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

    public IEnumerator DisableBetterJumpSpace(float time)
    {

        overrideBetterJumping = true;
        betterJumpEnabled = false;
        yield return new WaitForSeconds(time);
        overrideBetterJumping = false;
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
