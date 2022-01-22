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

        initRespawn();


    }

    void initRespawn()
    {

        PlayerMovement playerMovement = (PlayerMovement)FindObjectOfType(typeof(PlayerMovement));
        PlayerCameraAchor playerCameraAchor = (PlayerCameraAchor)FindObjectOfType(typeof(PlayerCameraAchor));

        if (GameManager.playerSpikeRespawnLocation != null)
        {

            Debug.Log("are we getting called in PlayerInit?");
            GameManager.playerSpikeRespawnLocation.playerlocation = playerMovement.transform.position;
            GameManager.playerSpikeRespawnLocation.cameraLocation = playerCameraAchor.transform.position;
            GameManager.playerSpikeRespawnLocation.cameraAnchorState = playerCameraAchor.anchorState;
            GameManager.playerSpikeRespawnLocation.stateHeight = playerCameraAchor.customHeight;
        }

        if (GameManager.playerDeathRespawnData != null)
        {
            GameManager.playerDeathRespawnData.playerlocation = playerMovement.transform.position;
            GameManager.playerDeathRespawnData.cameraLocation = playerCameraAchor.transform.position;
            GameManager.playerDeathRespawnData.cameraAnchorState = playerCameraAchor.anchorState;
            GameManager.playerDeathRespawnData.stateHeight = playerCameraAchor.customHeight;
        }




    }

}
