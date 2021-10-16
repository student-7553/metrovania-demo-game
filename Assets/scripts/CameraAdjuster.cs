using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraAdjuster : MonoBehaviour
{
    int playerLayer;
    public float firstHeight;
    public float secondHeight;


    // FollowToFixed
    // FixedToFollow
    // FixedToFixed
	[Tooltip("FollowToFixed,FixedToFollow,FixedToFixed")]
    public string firstToSecondTransition;


    public GameObject playerCameraAnchorObject;
    private PlayerCameraAchor playerCameraAnchor;

    private bool flipped;

    void Start()
    {
        playerCameraAnchor = playerCameraAnchorObject.gameObject.GetComponent<PlayerCameraAchor>();
        playerLayer = LayerMask.NameToLayer("Player");
        flipped = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != playerLayer)
        {
            return;
        }
        if (firstToSecondTransition == "FixedToFixed")
        {
            float setHeight;
            if (flipped)
            {
                setHeight = firstHeight;
            }
            else
            {
                setHeight = secondHeight;
            }
            playerCameraAnchor.anchorState = "SetHeight";
            playerCameraAnchor.setNewHeight(setHeight);
            flipped = !flipped;
        }
        else if (firstToSecondTransition == "FollowToFixed")
        {

        }
        else if (firstToSecondTransition == "FixedToFollow")
        {

        }


        // vcam.b
        // vcam.
    }

}
