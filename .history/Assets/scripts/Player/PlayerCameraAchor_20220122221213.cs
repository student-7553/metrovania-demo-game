using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// [DefaultExecutionOrder(100)]
public class PlayerCameraAchor : MonoBehaviour
{

    public GameObject player;

    [SerializeField]
    [Tooltip("Follow,SetHeight")]
    public string m_anchorState;

    [SerializeField]
    public float m_customHeight;
    // private bool manualOverrideHeight;

    private bool isLooking;
    private Vector2 lookingDirection;







    public string anchorState
    {
        get { return m_anchorState; }
    }
    public float customHeight
    {
        get { return m_customHeight; }

    }


    void Start()
    {
        // manualOverrideHeight = false;
        if (m_anchorState == "")
        {
            m_anchorState = "Follow";
        }

        Debug.Log("are we here?/" + m_anchorState+"/" + GameManager.playerSpikeRespawnLocation);

        if (GameManager.playerSpikeRespawnLocation != null)
        {
            transform.position = GameManager.playerSpikeRespawnLocation.cameraLocation;
            m_anchorState = GameManager.playerSpikeRespawnLocation.cameraAnchorState;
            m_customHeight = GameManager.playerSpikeRespawnLocation.stateHeight;
        }

    }

    // Update is called once per frame
    // void Update()
    void Update()
    {
        if (isLooking)
        {
            transform.position = lookingDirection;
            // isLooking overwrites default behavior 
        }
        else
        {
            if (m_anchorState == "SetHeight")
            {
                transform.position = new Vector2(player.transform.position.x, m_customHeight);
            }
            else if (m_anchorState == "Follow")
            {
                transform.position = new Vector2(player.transform.position.x, player.transform.position.y + m_customHeight);
            }
        }


    }

    public void updateStateAndHeight(string newAnchorState, float newHeight)
    {
        if (newAnchorState == "SetHeight")
        {
            m_anchorState = newAnchorState;
            m_customHeight = transform.position.y;
            DOTween.Kill("setNewHeight");
            // manualOverrideHeight = true;
            DOVirtual.Float(transform.position.y, newHeight, 1f, (float x) =>
            {

                m_customHeight = x;
            }).SetId("setNewHeight").OnComplete(() =>
            {
                // manualOverrideHeight = false;
            });
        }
        else if (newAnchorState == "Follow")
        {
            DOTween.Kill("setNewHeight");
            m_anchorState = newAnchorState;
            // m_customHeight = newHeight;

            float currentFollowHeight = this.transform.position.y - player.transform.position.y;
            m_customHeight = currentFollowHeight;

            DOTween.Kill("setNewHeight");
            DOVirtual.Float(currentFollowHeight, newHeight, 1f, (float x) =>
            {
                m_customHeight = x;
            }).SetId("setNewHeight");

        }
    }

    public void lookAtDirection(Vector2 direction)
    {
        isLooking = true;
        lookingDirection = (Vector2)transform.position + (direction * 4);

    }

    public void stopLooking()
    {

        isLooking = false;

    }



}
