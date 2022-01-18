using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerInput playerInput;
    private Rigidbody2D playerRigidBody;
    private PlayerCameraAchor playerCameraAnchor;
    private PlayerCollision playerCollision;
    public float lookAtBuffer = 1f;

    private float currentBufferDuration;
    private bool isLooking;
    private Vector2 lookingDirection;

    void Start()
    {
        playerCollision = GetComponent<PlayerCollision>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCameraAnchor = (PlayerCameraAchor)FindObjectOfType(typeof(PlayerCameraAchor));
        currentBufferDuration = 0f;
    }



    // Update is called once per frame
//     void Update()
//     {
//         if (!isLooking)
//         {
//             if ((Vector2)playerRigidBody.velocity == new Vector2(0f, 0f) && playerCollision.onGround)
//             {
//                 if (playerInput.vertical > 0.3f)
//                 {

//                     if (currentBufferDuration > lookAtBuffer)
//                     {
//                         isLooking = true;
//                         lookingDirection = Vector2.up;
//                         // trigger up
//                         playerCameraAnchor.lookAtDirection(lookingDirection);
//                     }
//                     else
//                     {
//                         currentBufferDuration = currentBufferDuration + Time.deltaTime;
//                     }




//                 }
//                 else if (playerInput.vertical < -0.3f)
//                 {

//                     if (currentBufferDuration > lookAtBuffer)
//                     {
//                         isLooking = true;
//                         lookingDirection = Vector2.down;
//                         // trigger up
//                         playerCameraAnchor.lookAtDirection(lookingDirection);
//                     }
//                     else
//                     {
//                         currentBufferDuration = currentBufferDuration + Time.deltaTime;
//                     }

//                 } else {
//                     currentBufferDuration = 0;
//                 }
//             }

//         }
//         else
//         {
//             if (
//                 !(
//                     playerRigidBody.velocity.x < 0.3f &&
//                     playerRigidBody.velocity.x > -0.3f &&
//                     playerRigidBody.velocity.y < 0.3f &&
//                     playerRigidBody.velocity.y > -0.3f
//                 ) ||

//                 !playerCollision.onGround)
//             {
//                 isLooking = false;
//                 lookingDirection = Vector2.zero;
//                 playerCameraAnchor.stopLooking();
//             }
//             else
//             {
//                 if (lookingDirection == Vector2.up && playerInput.vertical < 0.3f)
//                 {

//                     isLooking = false;
//                     lookingDirection = Vector2.zero;
//                     playerCameraAnchor.stopLooking();

//                 }
//                 else if (lookingDirection == Vector2.down && playerInput.vertical > -0.3f)
//                 {
//                     isLooking = false;
//                     lookingDirection = Vector2.zero;
//                     playerCameraAnchor.stopLooking();

//                 }
//             }

//         }



//     }
}
