using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraAdjusterPoint : MonoBehaviour
{
    int playerLayer;
    public float firstHeight;
    public float secondHeight;


    // FollowToFixed
    // FixedToFollow
    // FixedToFixed
    [Tooltip("FollowToFixed,FixedToFollow,FixedToFixed")]
    public string firstToSecondTransition;


    private PlayerCameraAchor playerCameraAnchor;

    public bool flipped;

    void Start()
    {

        playerCameraAnchor = (PlayerCameraAchor)FindObjectOfType(typeof(PlayerCameraAchor));
        playerLayer = LayerMask.NameToLayer("Player");
    
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


        }
        else if (firstToSecondTransition == "FollowToFixed")
        {

        }
        else if (firstToSecondTransition == "FixedToFollow")
        {

            if (flipped)
            {
                playerCameraAnchor.updateStateAndHeight("SetHeight", firstHeight);
            }
            else
            {
                playerCameraAnchor.updateStateAndHeight("Follow", secondHeight);
            }



        }
    }

}
