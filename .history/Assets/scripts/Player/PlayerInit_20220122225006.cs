using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerData.isAlive = true;

        initRespawn();

        string checkLastDeath = PlayerData.lastDeath;
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

    void initRespawn()
    {

        PlayerMovement playerMovement = (PlayerMovement)FindObjectOfType(typeof(PlayerMovement));
        PlayerCameraAchor playerCameraAchor = (PlayerCameraAchor)FindObjectOfType(typeof(PlayerCameraAchor));

        if (GameManager.playerSpikeRespawnLocation == null || GameManager.playerDeathRespawnData == null)
        {
            GameManager.awakeGameLogic();
        }





    }

}
