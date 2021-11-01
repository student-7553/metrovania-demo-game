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
    private string m_anchorState;

    // [SerializeField]
    // private float m_followHeight;
    // [SerializeField]
    // private float m_setHeight;
    [SerializeField]
    private float m_customHeight;
    private bool manualOverrideHeight;




    public string anchorState
    {
        get { return m_anchorState; }
        // set { m_anchorState = value; }
    }
    public float customHeight
    {
        get { return m_customHeight; }
        // set
        // {
        //     if (m_anchorState == "SetHeight")
        //     {
        //         // float oldHeight = m_customHeight;
        //         float oldHeight = transform.position.y;
        //         float newHeight = value;
        //         DOTween.Kill("setNewHeight");
        //         manualOverrideHeight = true;
        //         DOVirtual.Float(oldHeight, newHeight, 1f, (float x) =>
        //         {
                    
        //             m_customHeight = x;
        //         }).SetId("setNewHeight").OnComplete(() =>
        //         {
        //             manualOverrideHeight = false;
        //         });
        //     }
        //     else if (m_anchorState == "Follow")
        //     {
        //         // float oldHeight = m_customHeight;
        //         float newHeight = value;
        //         // DOTween.Kill("setNewHeight");
        //         m_customHeight = value;
        //         // manualOverrideHeight = true;
        //         // DOVirtual.Float(oldHeight, player.transform.position.y + newHeight, 1f, (float x) =>
        //         // {
        //         //     m_customHeight = x;
        //         // }).SetId("setNewHeight").OnComplete(() =>
        //         // {
        //         //     m_customHeight = newHeight;
        //         //     manualOverrideHeight = false;
        //         // });
        //     }


        // }
    
    }


    void Start()
    {
        manualOverrideHeight = false;
        if (m_anchorState == "")
        {
            m_anchorState = "Follow";
        }


        transform.position = GameManager.playerSpikeRespawnLocation.cameraLocation;
        m_anchorState = GameManager.playerSpikeRespawnLocation.cameraAnchorState;
        m_customHeight = GameManager.playerSpikeRespawnLocation.stateHeight;

    }

    // Update is called once per frame
    // void Update()
    void Update()
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

    public void updateStateAndHeight(string newAnchorState, float newHeight){
        if (newAnchorState == "SetHeight")
        {
            m_anchorState = newAnchorState;
            m_customHeight = transform.position.y;
            DOTween.Kill("setNewHeight");
            manualOverrideHeight = true;
            DOVirtual.Float(transform.position.y, newHeight, 1f, (float x) =>
            {
                
                m_customHeight = x;
            }).SetId("setNewHeight").OnComplete(() =>
            {
                manualOverrideHeight = false;
            });
        }
        else if (newAnchorState == "Follow")
        {
            m_anchorState = newAnchorState;
            m_customHeight = newHeight;

        }
    }





    // public void setNewHeight(float height){
    //     float oldHeight = m_setHeight;
    //     float newHeight = height;
    //     DOTween.Kill("setNewHeight");

    //     DOVirtual.Float(oldHeight, newHeight, 2f, (float x) => {
    //         m_setHeight = x;
    //     }).SetId("setNewHeight");
    // }
}
