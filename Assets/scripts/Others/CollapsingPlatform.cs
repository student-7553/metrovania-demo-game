using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    private int playerLayer;
    private bool initiated;

    public float destoyTime;
    public float restoreTime;
    private GameObject childPlatform;
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        childPlatform = this.gameObject.transform.GetChild(0).gameObject;
        initiated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != playerLayer || initiated){
            return;
        }
        initiated = true;
        childPlatform.BroadcastMessage("signalShake");
        StartCoroutine(breakDown());
        
    }
    IEnumerator breakDown(){
        yield return new WaitForSeconds(destoyTime);
        // childPlatform.BroadcastMessage("signalDestroy");
        childPlatform.SetActive(false);

        yield return new WaitForSeconds(restoreTime);
        // childPlatform.BroadcastMessage("signalRestore");
        childPlatform.SetActive(true);
        initiated = false;
    }
}
