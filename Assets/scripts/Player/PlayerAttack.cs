using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Space]
    [Header("Stats")]


    public int baseAttackDamage = 10;
    public int baseAttackFrameCount = 25;

    [Space]
    [Header("Booleans")]
    public bool isAttacking = false;



    private Rigidbody2D playerRigidBody;
    private PlayerMovement movement;
    private PlayerInput playerInput;
    private PlayerCollision playerCollision;
    private AnimationScript animationScript;
    public int attackFrameCounter = 0;

    // private int hitLayer;	

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
        movement = GetComponent<PlayerMovement>();
        // hitLayer = LayerMask.NameToLayer("hitInteractable");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.attackPressed && !isAttacking)
        {
            if (playerCollision.onGround)
            {
                BasicAttack();
            }

        }
    }
    private void BasicAttack()
    {

        animationScript.SetTrigger("attack");
        isAttacking = true;
        attackFrameCounter = 0;

        movement.canMove = false;
        
        playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);

        StartCoroutine(BasicAttackWait());

    }

    IEnumerator BasicAttackWait()
    {

        yield return WaitForFrames(2);

        Vector2 pos = transform.position;
		RaycastHit2D[] hits = Physics2D.RaycastAll(pos + new Vector2(1f , 0), Vector2.right, 1f);

        // Debug.DrawRay(pos + offset, rayDirection * length, debugCollisionColor);

        foreach (RaycastHit2D hit in hits) {
            // 
            if(hit.collider.gameObject.tag == "hitInteractable"){
                Debug.Log("Hit interactable");
                // hit.collider.gameObject.GetComponent<Interactable>
                // gameObject.SendMessage("ApplyDamage", 5.0);
                hit.collider.gameObject.SendMessage("onHit");
            }
        }






        yield return WaitForFrames(3);

        yield return WaitForFrames(baseAttackFrameCount - 5);
        isAttacking = false;
        movement.canMove = true;

        
        
    }

    public static IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
