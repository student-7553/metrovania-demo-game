using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionPoint : MonoBehaviour
{
    private int playerLayer;
    public int sceneIndex;
    public Vector2 playerlocation;
    public string cameraAnchorState;
    public float stateHeight;
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != playerLayer)
        {
            return;
        }

        GameManager.playerDeathRespawnData.playerlocation = playerlocation;
        GameManager.playerDeathRespawnData.cameraAnchorState = cameraAnchorState;
        GameManager.playerDeathRespawnData.stateHeight = stateHeight;


        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
