using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // if (GameManager.playerSpikeRespawnLocation != null)
        // {
        transform.position = GameManager.playerSpikeRespawnLocation.playerlocation;
        // }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
