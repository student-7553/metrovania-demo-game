using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroRange : MonoBehaviour
{
    // Start is called before the first frame update
    private int playerLayer;
    private BaseEnemy parentBaseEnemyScript;
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        parentBaseEnemyScript = GetComponentInParent<BaseEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {

            parentBaseEnemyScript.recieveAggroRange( collision.gameObject );

        }


    }
}
