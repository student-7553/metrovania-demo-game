using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerInit Start/" + GameManager.playerSpikeRespawnLocation.playerlocation);
        transform.position =  GameManager.playerSpikeRespawnLocation.playerlocation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
