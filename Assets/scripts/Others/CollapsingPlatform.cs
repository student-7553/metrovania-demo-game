using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    private int playerLayer;
    public bool initiated;
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
        // if(!isSquished){
        //     Debug.Log("we are here");
        //     isSquished = true;
        StartCoroutine(breakDown());
        // transform.position = new Vector2(transform.position.x, transform.position.y - 0.125f);
        // }
    }
    IEnumerator breakDown(){
        yield return new WaitForSeconds(1.5f);

        childPlatform.SetActive(false);

        yield return new WaitForSeconds(4f);
        Debug.Log("we are here");
        childPlatform.SetActive(true);
        initiated = false;
    }
}
