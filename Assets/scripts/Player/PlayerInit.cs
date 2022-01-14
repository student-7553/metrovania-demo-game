using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerData.isAlive = true;
        string checkLastDeath = PlayerData.lastDeath;
        // PlayerData.lastDeath = "";
        Debug.Log("checkLastDeath/"+checkLastDeath);
        if (checkLastDeath == "death")
        {
            if (GameManager.playerDeathRespawnData != null)
            {
                transform.position = GameManager.playerDeathRespawnData.playerlocation;
            }
        }
        else if (checkLastDeath == "trap")
        {
            if (GameManager.playerSpikeRespawnLocation != null)
            {
                transform.position = GameManager.playerSpikeRespawnLocation.playerlocation;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
