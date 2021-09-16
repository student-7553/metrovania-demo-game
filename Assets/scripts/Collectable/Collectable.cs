using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    int playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        GameManager.RegisterCollectable(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if(collision.gameObject.layer != playerLayer){
            return;
        }

        // AudioManager.
        GameManager.PlayerGrabbedCollectable(this);
        gameObject.SetActive(false);

        
    }
}
