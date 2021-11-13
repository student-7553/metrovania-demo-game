using System;
using System.Collections;
using System.Collections.Generic;
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


    [Space]
    [Header("CollisionData")]
    public float groundDistance;

    public bool drawDebugRay = false;
    private Color debugCollisionColor = Color.red;

    private Vector2 boxColliderSize;
    private Vector2 boxColliderOffset;



    public LayerMask groundLayerMask;
    private int groundLayer;
    private int trapsLayer;
    private int enemyLayer;

    private PlayerMovement playerMovement;



    void Start()
    {
 
        groundLayer = groundLayerMask.value;
      
        BoxCollider2D tempBox = GetComponent<BoxCollider2D>();
        boxColliderSize = tempBox.size;
        boxColliderOffset = tempBox.offset;

        trapsLayer = LayerMask.NameToLayer("Traps");
        enemyLayer = LayerMask.NameToLayer("Enemies");

        playerMovement = GetComponent<PlayerMovement>();

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
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == trapsLayer || !PlayerData.isAlive)
        {
            // Trap layer
            PlayerCollidedWithTrap();
        }
        else if (collision.gameObject.layer == enemyLayer || !PlayerData.isAlive)
        {
            // Enemy layer
            BaseEnemy tempBaseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            Debug.Log(tempBaseEnemy.damage);
            PlayerCollidedWithEnemy(tempBaseEnemy);
        }
        else
        {
            return;
        }
    }

    void PlayerCollidedWithEnemy(BaseEnemy enemy)
    {

        PlayerData.playerFloatResources.currentHealth = PlayerData.playerFloatResources.currentHealth - enemy.damage;
        Vector2 directionOfKnockBack = new Vector2();
        if(enemy.gameObject.transform.position.x > transform.position.x){
            // left
            directionOfKnockBack = Vector2.left;
        } else {
            // right
            directionOfKnockBack = Vector2.right;
        }
        
        playerMovement.knockBacked(directionOfKnockBack, 10f);
        //take knock back


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

            Debug.DrawRay(pos + offset, rayDirection * length, debugCollisionColor);
        }
        return hit;
    }
}
