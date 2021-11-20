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

    public bool drawDebugRay;

    public float attackAnimationCounter = 1;
    //  public LayerMask attackAbleLayer;	

    private Rigidbody2D playerRigidBody;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    private PlayerCollision playerCollision;
    private AnimationScript animationScript;
    private int baseAttackFrameCount = 21;

    [HideInInspector]
    private Color debugCollisionColor = Color.red;
    private int attackAbleLayerValue;


    // private int hitLayer;	

    void Start()
    {
        // attackAbleLayerValue = attackAbleLayer.value;
        attackAbleLayerValue = LayerMask.GetMask("Enemies");
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
        playerMovement = GetComponent<PlayerMovement>();

        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (playerInput.attackPressed && !isAttacking)
        {
            if (playerMovement.canMove)
            {
                BasicAttack();
            }

        }


    }
    private void BasicAttack()
    {

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

        isAttacking = true;

        animationScript.SetFloat("attackCounter", attackAnimationCounter);
        animationScript.SetTrigger("attack");

        playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);

        Vector2 lockedAxis = new Vector2(playerInput.horizontal, playerInput.vertical);
        StartCoroutine(BasicAttackWait(lockedAxis));
        StartCoroutine(BasicAttackGroundWait());

    }

    IEnumerator BasicAttackGroundWait()
    {
        while (isAttacking)
        {
            if (playerCollision.onGround)
            {
                playerMovement.canMove = false;
            }
            else
            {
                playerMovement.canMove = true;
            }

            yield return null;
        }
        playerMovement.canMove = true;
    }

    IEnumerator BasicAttackWait(Vector2 lockedAxis)
    {

        yield return WaitForFrames(2);

        Vector2 pos = transform.position;

        Vector2 direction;

        if (lockedAxis.y > 0.1f)
        {
            direction = Vector2.up;
        }
        else if (lockedAxis.y < -0.1f)
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

        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos + (direction * 2) + new Vector2(0f, 1f), 2.5f, direction, 0.5f, attackAbleLayerValue);


        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerData.playerFloatResources.currentBaseAttackDamage;
        tempStorage[1] = direction;

        // Debug.Log(hits.Length);

        if (hits.Length > 0 && direction == Vector2.down)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 30f);
            // StartCoroutine(playerMovement.DisableBetterJumpSpace(0.5f));

        }

        foreach (RaycastHit2D hit in hits)
        {
            // Debug.Log(hit.collider.name);
            if (!hit.collider.isTrigger)
            {

                hit.collider.gameObject.SendMessage("onHit", tempStorage);
                // hit.collider.gameObject.SendMessage("onHit",PlayerData.playerFloatResources.currentBaseAttackDamage, transform.position);
            }
        }

        yield return WaitForFrames(3);

        yield return WaitForFrames(baseAttackFrameCount - 5);
        isAttacking = false;
        playerMovement.canMove = true;



    }

    public void DashAttack(Vector2 prePosition, Vector2 postPosition)
    {
        Vector2 tempRemainder = postPosition - prePosition;
        float newX = 1f * Mathf.Sin(Mathf.Atan2(tempRemainder.x, tempRemainder.y));
        float newY = 1f * Mathf.Cos(Mathf.Atan2(tempRemainder.x, tempRemainder.y));
        Vector2 direction = new Vector2(newX, newY);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(prePosition, 2f, direction, Vector2.Distance(prePosition, postPosition), attackAbleLayerValue);
        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerData.playerFloatResources.currentBaseAttackDamage;
        tempStorage[1] = new Vector2(0f, 0f);

        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.isTrigger)
            {
                hit.collider.gameObject.SendMessage("onHit", tempStorage);
            }
        }
    }
    public static IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
