using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    int playerLayer;
    public Vector2 respawnPoint;
    public GameObject playerCameraAnchor;

    private PlayerCameraAchor playerCameraAnchorSript;

    void Start()
    {
        
        playerLayer = LayerMask.NameToLayer("Player");
        playerCameraAnchorSript = playerCameraAnchor.GetComponent<PlayerCameraAchor>();
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

        GameManager.playerSpikeRespawnLocation.playerlocation = respawnPoint;
        GameManager.playerSpikeRespawnLocation.cameraLocation = playerCameraAnchorSript.transform.position;
        GameManager.playerSpikeRespawnLocation.cameraAnchorState = playerCameraAnchorSript.anchorState;
        GameManager.playerSpikeRespawnLocation.stateHeight = playerCameraAnchorSript.customHeight;

        Debug.Log(GameManager.playerSpikeRespawnLocation.cameraLocation);

    }

}
