using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update

    // public int layer_mask;

    public LayerMask groundLayer;	
    
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

    private Vector2 leftLegOffset;
    private Vector2 rightLegOffset;

    private Vector2 leftWallOffset;
    private Vector2 rightWallOffset;

    private Vector2 leftWallOffsetBottom;
    private Vector2 rightWallOffsetBottom;

    private Vector2 leftWallOffsetTop;
    private Vector2 rightWallOffsetTop;


    public bool drawDebugRay = false;
    private Color debugCollisionColor = Color.red;

    private int collisionLayerValue;

    private Vector2 boxColliderSize;
    private Vector2 boxColliderOffset;


    void Start()
    {
        Debug.Log(collisionLayerValue);
        collisionLayerValue = groundLayer.value;
        // layer_mask = LayerMask.GetMask("Platform");
        BoxCollider2D tempBox = GetComponent<BoxCollider2D>();
        boxColliderSize = tempBox.size;
        boxColliderOffset = tempBox.offset;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 scale = transform.localScale;
        onGround = false;

        // leftLegOffset = new Vector2(-(scale.x / 2 - 0.2f) , -(scale.y / 2));
        leftLegOffset = new Vector2(-(boxColliderSize.x / 2 ) , 0);
        rightLegOffset = new Vector2((boxColliderSize.x / 2 ) , 0);


        leftWallOffset = new Vector2(-(boxColliderSize.x / 2) , boxColliderSize.y / 4);
        rightWallOffset = new Vector2((boxColliderSize.x / 2) ,  boxColliderSize.y / 4);


        leftWallOffsetBottom = new Vector2(-(boxColliderSize.x / 2) , 0);
        rightWallOffsetBottom = new Vector2((boxColliderSize.x / 2) , 0);
        
        leftWallOffsetTop = new Vector2(-(boxColliderSize.x / 2) , boxColliderSize.y);
        rightWallOffsetTop = new Vector2((boxColliderSize.x / 2) , boxColliderSize.y);


        RaycastHit2D leftLegHit = Raycast( leftLegOffset, Vector2.down, groundDistance,collisionLayerValue);
        RaycastHit2D rightLeftHit = Raycast( rightLegOffset, Vector2.down, groundDistance, collisionLayerValue);   

        if(leftLegHit || rightLeftHit){
            onGround = true;
        }
        
        RaycastHit2D leftWallHit = Raycast( leftWallOffset, Vector2.left, groundDistance, collisionLayerValue);
        RaycastHit2D rightWallHit = Raycast( rightWallOffset, Vector2.right, groundDistance,collisionLayerValue);  

        onRightWall = rightWallHit;
        onLeftWall = leftWallHit;

        RaycastHit2D leftWallBottomHit = Raycast( leftWallOffsetBottom, Vector2.left, groundDistance, collisionLayerValue);
        RaycastHit2D rightWallBottomHit = Raycast( rightWallOffsetBottom, Vector2.right, groundDistance, collisionLayerValue);  

        onLeftBottomWall = leftWallBottomHit;
        onRightBottomWall = rightWallBottomHit;

        RaycastHit2D leftWallTopHit = Raycast( leftWallOffsetTop, Vector2.left, groundDistance, collisionLayerValue);
        RaycastHit2D rightWallTopHit = Raycast( rightWallOffsetTop, Vector2.right, groundDistance, collisionLayerValue);  

        onLeftTopWall = leftWallTopHit;
        onRightTopWall = rightWallTopHit;


        onWall = false;

        // if((leftWallHit || rightWallHit) ){
        if((leftWallHit || rightWallHit) ||  (onLeftBottomWall || onRightBottomWall ) || ( onLeftTopWall || onRightTopWall )){
            onWall = true;
        }

        
        



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
