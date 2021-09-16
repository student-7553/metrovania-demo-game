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
    public Vector2 colliderSize;
    // public int wallSide;

    [Space]
    [Header("CollisionData")]
    public float groundDistance;

    private Vector2 leftLegOffset;
    private Vector2 rightLegOffset;

    private Vector2 leftWallOffset;
    private Vector2 rightWallOffset;

    private Vector2 leftWallOffsetBottom;
    private Vector2 rightWallOffsetBottom;


    public bool drawDebugRay = false;
    private Color debugCollisionColor = Color.red;

    private int collisionLayerValue;


    void Start()
    {
        collisionLayerValue = groundLayer.value;
        // layer_mask = LayerMask.GetMask("Platform");
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 scale = transform.localScale;
        onGround = false;

        // leftLegOffset = new Vector2(-(scale.x / 2 - 0.2f) , -(scale.y / 2));
        leftLegOffset = new Vector2(-(colliderSize.x/ 2 - 0.1f) , -(colliderSize.y / 2));
        rightLegOffset = new Vector2((colliderSize.x / 2 - 0.1f) , -(colliderSize.y / 2));

        leftWallOffset = new Vector2(-(colliderSize.x / 2) , 0);
        rightWallOffset = new Vector2((colliderSize.x / 2) ,  0);

        leftWallOffsetBottom = new Vector2(-(colliderSize.x / 2) , -(colliderSize.y / 2));
        rightWallOffsetBottom = new Vector2((colliderSize.x / 2) ,  -(colliderSize.y / 2));
        // rightWallOffset = new Vector2((scale.x / 2 - 0.1f) ,  -(scale.y / 2) );


        RaycastHit2D leftLegHit = Raycast( leftLegOffset, Vector2.down, groundDistance,collisionLayerValue);
        RaycastHit2D rightLeftHit = Raycast( rightLegOffset, Vector2.down, groundDistance, collisionLayerValue);   

        if(leftLegHit || rightLeftHit){
            onGround = true;
        }

        RaycastHit2D leftWallHit = Raycast( leftWallOffset, Vector2.left, groundDistance, collisionLayerValue);
        RaycastHit2D rightWallHit = Raycast( rightWallOffset, Vector2.right, groundDistance,collisionLayerValue);  
        

        onRightWall = rightWallHit;
        onLeftWall = leftWallHit;

        onWall = false;

        if(leftWallHit || rightWallHit){
            onWall = true;
        }

        RaycastHit2D leftWallBottomHit = Raycast( leftWallOffsetBottom, Vector2.left, groundDistance, collisionLayerValue);
        RaycastHit2D rightWallBottomHit = Raycast( rightWallOffsetBottom, Vector2.right, groundDistance, collisionLayerValue);  

        onLeftBottomWall = leftWallBottomHit;
        onRightBottomWall = rightWallBottomHit;
        



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
