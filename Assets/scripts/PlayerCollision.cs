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
    // public int wallSide;

    [Space]
    [Header("CollisionData")]
    public float groundDistance ;

    private Vector2 leftLegOffset;
    private Vector2 rightLegOffset;

    private Vector2 leftWallOffset;
    private Vector2 rightWallOffset;


    public bool drawDebugRay = false;
    private Color debugCollisionColor = Color.red;


    void Start()
    {
        // layer_mask = LayerMask.GetMask("Platform");
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 scale = transform.localScale;
        onGround = false;

        leftLegOffset = new Vector2(-(scale.x / 2) , -(scale.y / 2));
        rightLegOffset = new Vector2((scale.x / 2) , -(scale.y / 2));

        leftWallOffset = new Vector2(-(scale.x / 2) ,0);
        rightWallOffset = new Vector2((scale.x / 2) , 0);


        RaycastHit2D leftLegHit = Raycast( leftLegOffset, Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightLeftHit = Raycast( rightLegOffset, Vector2.down, groundDistance, groundLayer);   

        if(leftLegHit || rightLeftHit){
            onGround = true;
        }

        RaycastHit2D leftWallHit = Raycast( leftWallOffset, Vector2.left, groundDistance, groundLayer);
        RaycastHit2D rightWallHit = Raycast( rightWallOffset, Vector2.right, groundDistance, groundLayer);  

        onRightWall = rightWallHit;
        onLeftWall = leftWallHit;

        onWall = false;

        if(leftWallHit || rightWallHit){
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
