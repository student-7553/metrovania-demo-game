using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    int playerLayer;
    public Vector2 respawnPoint;

    void Start()
    {
        
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
        
        Vector2 diffrenceTransform =  transform.position - collision.gameObject.transform.position;

        GameManager.playerSpikeRespawnLocation = respawnPoint ;

    }

}
