using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update

    int layer_mask;
    
    [Space]
    [Header("State")]
    public bool onGround;
    // public bool onWall;
    // public bool onRightWall;
    // public bool onLeftWall;
    // public int wallSide;

    [Space]
    [Header("Collision")]
    public float groundDistance = .2f;
    public Vector2 leftLegOffset;
    public Vector2 rightLegOffset;


    public bool drawDebugRay = false;
    private Color debugCollisionColor = Color.red;


    void Start()
    {
        layer_mask = LayerMask.GetMask("Platform");
    }
    // Update is called once per frame
    void Update()
    {

        RaycastHit2D leftLegHit = Raycast( leftLegOffset, Vector2.down, groundDistance, layer_mask);
        RaycastHit2D rightLeftHit = Raycast( rightLegOffset, Vector2.down, groundDistance, layer_mask);
        if(leftLegHit || rightLeftHit){
            onGround = true;
        }

    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, int mask)
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
