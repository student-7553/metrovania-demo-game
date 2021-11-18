using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerPathing : MonoBehaviour
{
    // Start is called before the first frame update
    public int playerLayer;
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.layer == playerLayer)
        {
            // Debug.Log("yeppp");
            Debug.Log("ChargerPathing/" + collision.gameObject.name);
        }


    }
}
