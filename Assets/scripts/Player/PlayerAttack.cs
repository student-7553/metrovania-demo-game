using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // [Space]
    // [Header("Stats")]


    [Space]
    [Header("Booleans")]

    public bool isAttacking;
    public bool isSpiritRangedAttacking, isSpiritMeleeAttacking, isBasicAttacking, isParrying;

    public float attackAnimationCounter = 1;
    public int baseAttackFrameCount = 25;

    public GameObject spiritOrb;

    public float attackCoolDownTimer;

    public float parryDuration;


    private Rigidbody2D playerRigidBody;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    private PlayerCollision playerCollision;
    private AnimationScript animationScript;

    [HideInInspector]
    private Color debugCollisionColor = Color.red;
    private int enemyHitBoxLayerMask, groundWithAttackableLayerMask;

    private int enemyHitBoxLayerIndex;
    private bool rangedBombPreping;
    private float timer = 0f;
    private bool parryTriggered;



    // private int hitLayer;	

    void Start()
    {

        enemyHitBoxLayerMask = LayerMask.GetMask("EnemyHitBox");
        enemyHitBoxLayerIndex = LayerMask.NameToLayer("EnemyHitBox");

        string[] tempLayers = { "EnemyHitBox", "Platform" };
        groundWithAttackableLayerMask = LayerMask.GetMask(tempLayers);




        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
        playerMovement = GetComponent<PlayerMovement>();

        isBasicAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.canMove)
        {
            if (
                playerInput.focusHeld &&
                playerInput.rangedPressed &&
                !isAttacking)
            {
                // ranged spirit attack
                if (
                    PlayerData.playerBoolUpgrades.isRangedSpiritAttackAvailable &&
                    PlayerData.playerFloatResources.currentMana >= PlayerData.playerResourceUsage.lance)
                {
                    PlayerData.playerFloatResources.currentMana = PlayerData.playerFloatResources.currentMana - PlayerData.playerResourceUsage.lance;
                    rangedSpiritAttack(playerInput.horizontal, playerInput.vertical);
                }
            }



            if (
                playerInput.focusHeld &&
                playerInput.attackPressed &&
                !isAttacking)
            {
                // melee heavy spirit attack
                if (
                    PlayerData.playerBoolUpgrades.isRangedSpiritAttackAvailable &&
                    PlayerData.playerFloatResources.currentMana >= PlayerData.playerResourceUsage.focusStrike)
                {
                    PlayerData.playerFloatResources.currentMana = PlayerData.playerFloatResources.currentMana - PlayerData.playerResourceUsage.focusStrike;

                    StartCoroutine(meleeSpiritAttack());
                }
            }

            if (
                playerInput.focusHeld &&
                playerInput.rangedBombPressed &&
                !rangedBombPreping &&
                !isAttacking
                )
            {

                // ranged bomb prep

                isAttacking = true;
                rangedBombPreping = true;
                playerMovement.canMove = false;
                playerRigidBody.velocity = new Vector2(0f, 0f);
                playerRigidBody.gravityScale = 0;
                playerMovement.overrideBetterJumping = true;
            }


            if (
                playerInput.parryPressed &&
                playerCollision.onGround &&
                !isAttacking
                )
            {

                StartCoroutine(Parry());
            }

            if (playerInput.attackPressed && !isBasicAttacking && !isAttacking)
            {
                // basic attack
                BasicAttack();
            }


        }

        preppingAttack();

    }

    private void preppingAttack()
    {
        if (rangedBombPreping)
        {
            if (
            !playerInput.rangedBombHeld &&
            timer > 1f
          )
            {

                // throw the spirit orb
                timer = 0f;
                rangedBombPreping = false;
                playerMovement.overrideBetterJumping = false;
                isAttacking = false;
                playerRigidBody.gravityScale = 3;
                playerMovement.canMove = true;

                GameObject newSpiritOrb = Instantiate(spiritOrb, transform.position + new Vector3(0f, 1f), Quaternion.identity);
                newSpiritOrb.GetComponent<Rigidbody2D>().velocity = new Vector2(playerInput.horizontalSoft * 25, playerInput.verticalSoft * 20);
            }
            else if (
                !playerInput.rangedBombHeld &&
                timer < 1f
                )
            {
                // reset stance on spirit orb

                timer = 0f;
                rangedBombPreping = false;
                playerMovement.overrideBetterJumping = false;
                isAttacking = false;
                playerRigidBody.gravityScale = 3;
                playerMovement.canMove = true;
            }
            else
            {
                timer = timer + Time.deltaTime;
            }
        }


    }

    private void BasicAttack()
    {
        if (!PlayerData.playerBoolUpgrades.isAttackAvailable)
        {
            return;
        }

        if (attackAnimationCounter > 0)
        {
            attackAnimationCounter = -1;
        }
        else
        {
            attackAnimationCounter = 1;
        }

        if (!playerCollision.onGround)
        {
            attackAnimationCounter = -1;
        }



        isBasicAttacking = true;
        isAttacking = true;

        animationScript.SetFloat("attackCounter", attackAnimationCounter);
        animationScript.SetTrigger("attack");

        playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);

        // playerMovement.canMove = false;

        if (playerCollision.onGround)
        {
            playerMovement.canMove = false;
        }
        else
        {
            playerMovement.canMove = true;
        }

        Vector2 lockedAxis = new Vector2(playerInput.horizontal, playerInput.vertical);
        StartCoroutine(BasicAttackWait(lockedAxis));
        // StartCoroutine(BasicAttackGroundWait());

    }



    private IEnumerator Parry()
    {

        isAttacking = true;
        isParrying = true;
        playerMovement.canMove = false;
        playerRigidBody.velocity = new Vector2(0f, 0f);

        yield return new WaitForSeconds(parryDuration);

        if (parryTriggered)
        {
            parryTriggered = false;
        }
        else
        {
            isParrying = false;
            isAttacking = false;
            playerMovement.canMove = true;
        }



    }

    public void ParryTrigger(bool isHeavy)
    {
        parryTriggered = true;
        if (isHeavy)
        {
            // if heavy just take the knockback with no damage
        }
        else
        {
            StartCoroutine(ParryLight());
            // light
            // trigger animation


        }


    }

    private IEnumerator ParryLight()
    {
        // trigger animation

        //  SendMessage to all enemies around to knock them back and stagger them
        RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2)this.transform.position, 1.5f, Vector2.zero, 0f, enemyHitBoxLayerMask);


        foreach (RaycastHit2D hit in hits)
        {

            if (hit && hit.collider.gameObject.layer == enemyHitBoxLayerIndex)
            {

                object[] tempStorage = new object[2];

                tempStorage[0] = PlayerData.playerFloatResources.baseAttackDamage;
                tempStorage[1] = new Vector2(0f, 1f);


                hit.collider.gameObject.transform.parent.gameObject.SendMessage("onDeflect", tempStorage);
            }
        }



        yield return new WaitForSeconds(0.1f);

        isParrying = false;
        isAttacking = false;
        playerMovement.canMove = true;
    }

    private IEnumerator BasicAttackWait(Vector2 lockedAxis)
    {

        yield return WaitForFrames(2);

        Vector2 pos = transform.position;

        Vector2 direction;

        if (lockedAxis.y > 0.1f)
        {
            direction = Vector2.up;
        }
        else if (lockedAxis.y < -0.1f && !playerCollision.onGround)
        {
            direction = Vector2.down;
        }
        else
        {

            if (lockedAxis.x > 0.1f)
            {
                direction = Vector2.right;
            }
            else if (lockedAxis.x < -0.1f)
            {
                direction = Vector2.left;
            }
            else
            {
                if (playerMovement.isFacingRight)
                {
                    direction = Vector2.right;
                }
                else
                {
                    direction = Vector2.left;
                }
            }
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos + (direction * 2) + new Vector2(0f, 1f), 2.5f, direction, 0.5f, enemyHitBoxLayerMask);


        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerData.playerFloatResources.currentBaseAttackDamage;
        tempStorage[1] = direction;

        if (hits.Length > 0 && direction == Vector2.down)
        {
            playerMovement.PlayerUpBump();
        }

        foreach (RaycastHit2D hit in hits)
        {
            hit.collider.gameObject.transform.parent.gameObject.SendMessage("onHit", tempStorage);
        }

        yield return WaitForFrames(3);

        yield return WaitForFrames(baseAttackFrameCount - 5);

        playerMovement.canMove = true;
        isAttacking = false;


        yield return new WaitForSeconds(attackCoolDownTimer);


        isBasicAttacking = false;



    }

    public void DashAttack(Vector2 prePosition, Vector2 postPosition)
    {
        Vector2 tempRemainder = postPosition - prePosition;
        float newX = 1f * Mathf.Sin(Mathf.Atan2(tempRemainder.x, tempRemainder.y));
        float newY = 1f * Mathf.Cos(Mathf.Atan2(tempRemainder.x, tempRemainder.y));
        Vector2 direction = new Vector2(newX, newY);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(prePosition, 2f, direction, Vector2.Distance(prePosition, postPosition), enemyHitBoxLayerMask);
        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerData.playerFloatResources.currentBaseAttackDamage;
        tempStorage[1] = new Vector2(0f, 0f);

        foreach (RaycastHit2D hit in hits)
        {
            // if (!hit.collider.isTrigger)
            // {
            hit.collider.gameObject.transform.parent.gameObject.SendMessage("onHit", tempStorage);
            // hit.collider.gameObject.SendMessage("onHit", tempStorage);
            // }
        }
    }

    private void rangedSpiritAttack(float xAxis, float yAxis)
    {
        isAttacking = true;
        isSpiritRangedAttacking = true;

        Vector2 direction = new Vector2(xAxis, yAxis);

        StartCoroutine(playerMovement.SpiritRangedAttackShift(direction));

        StartCoroutine(rangedSpiritAttackAfter(direction));


    }

    private IEnumerator rangedSpiritAttackAfter(Vector2 direction)
    {
        yield return new WaitForSeconds(0.35f);

        RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2)this.transform.position + new Vector2(0f, 1f), 1.5f, direction, 10f, groundWithAttackableLayerMask);

        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerData.playerFloatResources.currentSpiritRangedAttackDamage;
        tempStorage[1] = direction;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit && hit.collider.gameObject.layer == enemyHitBoxLayerIndex)
            {

                // hit.collider.gameObject.transform.parent.gameObject.SendMessage("onHit", tempStorage);
                hit.collider.gameObject.transform.parent.gameObject.SendMessage("onHit", tempStorage);
            }
        }


        isAttacking = false;
        isSpiritRangedAttacking = false;
        playerMovement.canMove = true;
    }



    private IEnumerator meleeSpiritAttack()
    {

        if (!PlayerData.playerBoolUpgrades.iSpiritMeleeAttackAvailable)
        {
            yield break;
        }

        isAttacking = true;
        isSpiritMeleeAttacking = true;
        Vector2 direction = new Vector2(playerMovement.isFacingRight ? 1f : -1f, 0);
        playerRigidBody.velocity = new Vector2(0f, 0f);
        playerRigidBody.gravityScale = 0;
        playerMovement.canMove = false;
        playerMovement.overrideBetterJumping = true;

        yield return new WaitForSeconds(0.2f);

        RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2)transform.position + (direction * 2) + new Vector2(0f, 1f), 2.5f, direction, 2.5f, enemyHitBoxLayerMask);


        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerData.playerFloatResources.currentSpiritMeleeAttackDamage;
        tempStorage[1] = direction;


        foreach (RaycastHit2D hit in hits)
        {
            // if (!hit.collider.isTrigger)
            // {

            hit.collider.gameObject.transform.parent.gameObject.SendMessage("onHit", tempStorage);
            // }
        }

        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        isSpiritMeleeAttacking = false;
        playerRigidBody.gravityScale = 3;
        playerMovement.overrideBetterJumping = false;
        playerMovement.canMove = true;
    }

    private static IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
