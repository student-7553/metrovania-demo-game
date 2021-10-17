using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCameraAchor : MonoBehaviour
{

    public GameObject player;

    [SerializeField]
    [Tooltip("Follow,SetHeight")]
    private string m_anchorState;

    [SerializeField]
    private float m_followHeight;
    [SerializeField]
    private float m_setHeight;



    public string anchorState
    {
        get {return m_anchorState; }
        set {m_anchorState = value; }
    }
    public float followHeight
    {
        get {return m_followHeight; }
        set {
            // m_followHeight = value; 
            float oldHeight = m_followHeight;
            float newHeight = value;
            DOTween.Kill("setNewHeight");

            DOVirtual.Float(oldHeight, newHeight, 2f, (float x) => {
                m_followHeight = x;
            }).SetId("setNewHeight");
            
        }
    }

    public float setHeight
    {
        get {return m_setHeight; }
        set {
            float oldHeight = m_setHeight;
            float newHeight = value;
            DOTween.Kill("setNewHeight");

            DOVirtual.Float(oldHeight, newHeight, 2f, (float x) => {
                m_setHeight = x;
            }).SetId("setNewHeight");

        }

    }
    void Start()
    {
        if(m_anchorState == ""){
            m_anchorState = "Follow";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_anchorState == "SetHeight"){
            transform.position = new Vector2( player.transform.position.x, m_setHeight);
        } else if (m_anchorState == "Follow"){
            transform.position = new Vector2( player.transform.position.x, player.transform.position.y + m_followHeight);
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
