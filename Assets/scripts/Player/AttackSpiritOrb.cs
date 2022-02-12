using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpiritOrb : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D thisRigidBody;
    SpriteRenderer thisSpriteRenderer;
    private int enemyHitBoxLayer, enemyHitBoxLayerMask;

    void Start()
    {

        enemyHitBoxLayer = LayerMask.NameToLayer("EnemyHitBox");
        enemyHitBoxLayerMask = LayerMask.GetMask("EnemyHitBox");

        thisRigidBody = GetComponent<Rigidbody2D>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        thisRigidBody.isKinematic = true;
        thisSpriteRenderer.enabled = false;
        // trigger the explosion animation
        spiritBombExplosion();
        // regsiter the damage

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // hit an enemy
        if (collision.gameObject.layer != enemyHitBoxLayer)
        {

            return;
        }

        thisRigidBody.isKinematic = true;
        thisSpriteRenderer.enabled = false;
        spiritBombExplosion();
    }

    private void spiritBombExplosion()
    {
        // this.transform

        RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2)this.transform.position, 1.5f, Vector2.zero, 0f, enemyHitBoxLayerMask);


        foreach (RaycastHit2D hit in hits)
        {
            
            if (hit && hit.collider.gameObject.layer == enemyHitBoxLayer)
            {

                object[] tempStorage = new object[2];
                tempStorage[0] = PlayerData.playerFloatResources.currentSpiritMeleeAttackDamage;
                tempStorage[1] = new Vector2(0f, 1f);


                hit.collider.gameObject.transform.parent.gameObject.SendMessage("onHit", tempStorage);
            }
        }


    }
}
