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

    public bool flipped;

    void Start()
    {
        playerCameraAnchor = playerCameraAnchorObject.gameObject.GetComponent<PlayerCameraAchor>();
        playerLayer = LayerMask.NameToLayer("Player");
        // flipped = false;
    }

    void Update()
    {

    }

    void OnTriggerExit2D(Collider2D collision)
    {


        if (collision.gameObject.layer != playerLayer)
        {
            return;
        }

        Vector2 diffrenceTransform = transform.position - collision.gameObject.transform.position;

        if (diffrenceTransform.x > 0.5f)
        {
            flipped = true;
        }
        else
        {
            flipped = false;
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
            playerCameraAnchor.updateStateAndHeight("SetHeight", setHeight);
            // playerCameraAnchor.anchorState = "SetHeight";
            // playerCameraAnchor.customHeight = setHeight;

        }
        else if (firstToSecondTransition == "FollowToFixed")
        {

        }
        else if (firstToSecondTransition == "FixedToFollow")
        {

            if (flipped)
            {
                playerCameraAnchor.updateStateAndHeight("SetHeight", firstHeight);
                // playerCameraAnchor.customHeight = firstHeight;
                // playerCameraAnchor.anchorState = "SetHeight";

            }
            else
            {
                playerCameraAnchor.updateStateAndHeight("Follow", secondHeight);
                // playerCameraAnchor.customHeight = secondHeight;
                // playerCameraAnchor.anchorState = "Follow";

            }



        }


        // vcam.b
        // vcam.
    }

}
