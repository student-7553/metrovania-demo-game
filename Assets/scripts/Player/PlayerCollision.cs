using System.Collections;
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
    private int enemyHitBoxLayer;

    [System.NonSerialized]
    public bool allowEnemyTrigger = true;


    private PlayerMovement playerMovement;



    void Start()
    {



        BoxCollider2D tempBox = GetComponent<BoxCollider2D>();
        boxColliderSize = tempBox.size;
        boxColliderOffset = tempBox.offset;


        groundLayer = LayerMask.GetMask("Platform");
        trapsLayer = LayerMask.NameToLayer("Traps");
        enemyLayer = LayerMask.NameToLayer("Enemies");
        enemyHitBoxLayer = LayerMask.NameToLayer("EnemyHitBox");
        playerLayer = LayerMask.NameToLayer("Player");


        playerMovement = GetComponent<PlayerMovement>();

        enabledCollision = true;
        allowEnemyTrigger = true;

        // Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

    }
    // Update is called once per frame
    void Update()
    {
        collisionRays();

    }

    void collisionRays()
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
    private void OnCollisionEnter2D(Collision2D collision)
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
        // else if (collision.gameObject.layer == enemyLayer)
        // {
        //     // Enemy layer
        //     BaseEnemy tempBaseEnemy = collision.gameObject.GetComponentInParent<BaseEnemy>();

        //     PlayerCollidedWithEnemy(tempBaseEnemy);
        // }
        else
        {
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!PlayerData.isAlive)
        {
            return;
        }
        // START FOCUS DASHING HIT ENEMY MID WAY
        if(collision.gameObject.layer == enemyHitBoxLayer && playerMovement.isFocusDashing){

            object[] tempStorage = new object[2];
            tempStorage[0] = PlayerData.playerFloatResources.currentBaseAttackDamage;
            tempStorage[1] = new Vector2(0f, 0f);
            // 
            BaseEnemy tempBaseEnemy = collision.gameObject.GetComponentInParent<BaseEnemy>();
            tempBaseEnemy.gameObject.SendMessage("onHit", tempStorage);

            if (playerMovement.isDashing)
            {
                playerMovement.DashEscape();
            }

            Vector2 directionOfKnockBack = new Vector2();

            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                // left
                directionOfKnockBack = new Vector2( -0.8f, 0.8f);
            }
            else
            {
                // right
                directionOfKnockBack = new Vector2( 0.8f, 0.8f);
            }

            StartCoroutine(playerMovement.knockBackPlayer(directionOfKnockBack, 10f, 0.2f));



        }

        // END FOCUS DASHING HIT ENEMY MID WAY
        
        if (collision.gameObject.layer == enemyHitBoxLayer && allowEnemyTrigger)
        {
            BaseEnemy tempBaseEnemy = collision.gameObject.GetComponentInParent<BaseEnemy>();
            if (tempBaseEnemy.isAlive)
            {
              
                PlayerCollidedWithEnemy(tempBaseEnemy);
            }
            else
            {
                return;
            }
        }
        // projectile
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

        Debug.Log("got hit");

        StartCoroutine(playerMovement.knockBackPlayer(directionOfKnockBack, 10f, 0.5f));

        StartCoroutine(disableCollisionForTime(eyeFrameLength));


    }

    void PlayerCollidedWithTrap()
    {
        
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
        // Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        allowEnemyTrigger = false;
        // Physics2D.
        enabledCollision = false;
        yield return new WaitForSeconds(time);
        // Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        allowEnemyTrigger = true;
        enabledCollision = true;


    }
}
