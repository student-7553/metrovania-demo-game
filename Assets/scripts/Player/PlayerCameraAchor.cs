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



    public string anchorState
    {
        get {return m_anchorState; }
        set {m_anchorState = value; }
    }
    public float customHeight
    {
        get {return m_customHeight; }
        set {
            
            float oldHeight = m_customHeight;
            float newHeight = value;
            DOTween.Kill("setNewHeight");

            DOVirtual.Float(oldHeight, newHeight, 2f, (float x) => {
                m_customHeight = x;
            }).SetId("setNewHeight");
            
        }
    }

    // public float setHeight
    // {
    //     get {return m_setHeight; }
    //     set {
    //         float oldHeight = m_setHeight;
    //         float newHeight = value;
    //         DOTween.Kill("setNewHeight");

    //         DOVirtual.Float(oldHeight, newHeight, 2f, (float x) => {
    //             m_setHeight = x;
    //         }).SetId("setNewHeight");

    //     }

    // }
    void Start()
    {
        if(m_anchorState == ""){
            m_anchorState = "Follow";
        }
        Debug.Log("PlayerCameraAchor Starting/"+GameManager.playerSpikeRespawnLocation.cameraLocation);

        transform.position = GameManager.playerSpikeRespawnLocation.cameraLocation;
        m_anchorState = GameManager.playerSpikeRespawnLocation.cameraAnchorState;
        m_customHeight = GameManager.playerSpikeRespawnLocation.stateHeight;

    }

    // Update is called once per frame
    void Update()
    {
        if(m_anchorState == "SetHeight"){
            transform.position = new Vector2( player.transform.position.x, m_customHeight);
        } else if (m_anchorState == "Follow"){
            transform.position = new Vector2( player.transform.position.x, player.transform.position.y + m_customHeight);
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
