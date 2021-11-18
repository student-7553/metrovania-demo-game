﻿using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Space]
    [Header("State")]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onRightBottomWall;
    public bool onLeftBottomWall;
    public bool onRightTopWall;
    public bool onLeftTopWall;
    public float eyeFrameLength;


    [Space]
    [Header("CollisionData")]
    public float groundDistance;
    public bool drawDebugRay = false;
    public bool enabledCollision = true;


    private Vector2 boxColliderSize;
    private Vector2 boxColliderOffset;



    private int groundLayer;
    private int trapsLayer;
    private int enemyLayer;
    private int playerLayer;


    private PlayerMovement playerMovement;



    void Start()
    {



        BoxCollider2D tempBox = GetComponent<BoxCollider2D>();
        boxColliderSize = tempBox.size;
        boxColliderOffset = tempBox.offset;


        groundLayer = LayerMask.GetMask("Platform");
        trapsLayer = LayerMask.NameToLayer("Traps");
        enemyLayer = LayerMask.NameToLayer("Enemies");
        playerLayer = LayerMask.NameToLayer("Player");


        playerMovement = GetComponent<PlayerMovement>();

        enabledCollision = true;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

    }
    // Update is called once per frame
    void Update()
    {
        drawCollisionRays();

    }

    void drawCollisionRays()
    {
        onGround = false;

        Vector2 leftLegOffset = new Vector2(-(boxColliderSize.x / 2), 0);
        Vector2 rightLegOffset = new Vector2((boxColliderSize.x / 2), 0);


        Vector2 leftWallOffset = new Vector2(-(boxColliderSize.x / 2), boxColliderSize.y / 4);
        Vector2 rightWallOffset = new Vector2((boxColliderSize.x / 2), boxColliderSize.y / 4);


        Vector2 leftWallOffsetBottom = new Vector2(-(boxColliderSize.x / 2), 0);
        Vector2 rightWallOffsetBottom = new Vector2((boxColliderSize.x / 2), 0);

        Vector2 leftWallOffsetTop = new Vector2(-(boxColliderSize.x / 2), boxColliderSize.y);
        Vector2 rightWallOffsetTop = new Vector2((boxColliderSize.x / 2), boxColliderSize.y);


        RaycastHit2D leftLegHit = Raycast(leftLegOffset, Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightLeftHit = Raycast(rightLegOffset, Vector2.down, groundDistance, groundLayer);

        if (leftLegHit || rightLeftHit)
        {
            onGround = true;
        }

        RaycastHit2D leftWallHit = Raycast(leftWallOffset, Vector2.left, groundDistance, groundLayer);
        RaycastHit2D rightWallHit = Raycast(rightWallOffset, Vector2.right, groundDistance, groundLayer);

        onRightWall = rightWallHit;
        onLeftWall = leftWallHit;

        RaycastHit2D leftWallBottomHit = Raycast(leftWallOffsetBottom, Vector2.left, groundDistance, groundLayer);
        RaycastHit2D rightWallBottomHit = Raycast(rightWallOffsetBottom, Vector2.right, groundDistance, groundLayer);

        onLeftBottomWall = leftWallBottomHit;
        onRightBottomWall = rightWallBottomHit;

        RaycastHit2D leftWallTopHit = Raycast(leftWallOffsetTop, Vector2.left, groundDistance, groundLayer);
        RaycastHit2D rightWallTopHit = Raycast(rightWallOffsetTop, Vector2.right, groundDistance, groundLayer);

        onLeftTopWall = leftWallTopHit;
        onRightTopWall = rightWallTopHit;


        onWall = false;

        if ((leftWallHit || rightWallHit) || (onLeftBottomWall || onRightBottomWall) || (onLeftTopWall || onRightTopWall))
        {
            onWall = true;
        }
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(!enabledCollision){
    //         if( collision.gameObject.layer == enemyLayer ){
    //             Debug.Log("are we here?/"+ collision.gameObject.name);
    //             Physics2D.IgnoreCollision( collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    //         }
    //     }
    // }
    void OnCollisionStay2D(Collision2D collision)
    {

        if (!enabledCollision || !PlayerData.isAlive)
        {
            return;
        }

        if (collision.gameObject.layer == trapsLayer)
        {
            // Trap layer
            PlayerCollidedWithTrap();
        }
        else if (collision.gameObject.layer == enemyLayer)
        {
            // Enemy layer
            BaseEnemy tempBaseEnemy = collision.gameObject.GetComponentInParent<BaseEnemy>();
            // Debug.Log(tempBaseEnemy.damage);
            PlayerCollidedWithEnemy(tempBaseEnemy);
        }
        else
        {
            return;
        }
    }

    public void PlayerCollidedWithEnemy(BaseEnemy enemy)
    {

        PlayerData.playerFloatResources.currentHealth = PlayerData.playerFloatResources.currentHealth - enemy.damage;
        Vector2 directionOfKnockBack = new Vector2();
        if (enemy.gameObject.transform.position.x > transform.position.x)
        {
            // left
            directionOfKnockBack = Vector2.left;
        }
        else
        {
            // right
            directionOfKnockBack = Vector2.right;
        }
        if (playerMovement.isDashing)
        {
            playerMovement.DashEscape();
        }

        // Debug.Log(directionOfKnockBack);
        StartCoroutine(playerMovement.knockBackPlayer(directionOfKnockBack, 10f));

        StartCoroutine(disableCollisionForTime(eyeFrameLength));


    }

    void PlayerCollidedWithTrap()
    {
        PlayerData.isAlive = false;
        gameObject.SetActive(false);
        GameManager.PlayerHitTrap();
        AudioManager.PlayDeathAudio();
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {

        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        if (drawDebugRay)
        {

            Debug.DrawRay(pos + offset, rayDirection * length, Color.red);
        }
        return hit;
    }

    public IEnumerator disableCollisionForTime(float time)
    {
        // Physics2D.IgnoreCollision( collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        // Physics2D.
        enabledCollision = false;
        yield return new WaitForSeconds(time);
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        enabledCollision = true;

    }
}
