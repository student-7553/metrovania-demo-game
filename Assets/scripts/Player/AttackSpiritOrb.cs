using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpiritOrb : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D thisRigidBody;
    SpriteRenderer thisSpriteRenderer;

    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody2D>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("collision/"+collision.gameObject.name);

        thisRigidBody.isKinematic = true;
        thisSpriteRenderer.enabled = false;
        // trigger the explosion animation

        // regsiter the damage

    }
}
