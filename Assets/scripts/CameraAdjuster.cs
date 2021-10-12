using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAdjuster : MonoBehaviour
{

    public CinemachineVirtualCamera vcam;
    int playerLayer;					
    
	void Start()
	{
		playerLayer = LayerMask.NameToLayer("Player");
	}

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer != playerLayer )
			return;

		Debug.Log("are we here?");
        // vcam.b
        // vcam.
	}

}
