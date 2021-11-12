using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Space]
    [Header("Stats")]


    
    public int baseAttackFrameCount;

    [Space]
    [Header("Booleans")]
    public bool isAttacking;

    private Rigidbody2D playerRigidBody;
    private PlayerMovement movement;
    private PlayerInput playerInput;
    private PlayerCollision playerCollision;
    private AnimationScript animationScript;
    private PlayerStat playerStat;
    
    [HideInInspector]
    public float attackAnimationCounter;

    public bool drawDebugRay;
    private Color debugCollisionColor = Color.red;

    public LayerMask attackAbleLayer;	

    private int attackAbleLayerValue;


    // private int hitLayer;	

    void Start()
    {
        attackAbleLayerValue = attackAbleLayer.value;

        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCollision = GetComponent<PlayerCollision>();
        animationScript = GetComponentInChildren<AnimationScript>();
        movement = GetComponent<PlayerMovement>();
        playerStat = GetComponent<PlayerStat>();
        attackAnimationCounter = 1;

        isAttacking = false;
        // hitLayer = LayerMask.NameToLayer("hitInteractable");
    }

    // Update is called once per frame
    void Update()
    {


        if (playerInput.attackPressed && !isAttacking)
        {
            if (movement.canMove)
            {
                BasicAttack();
            }

        }

        if (drawDebugRay)
		{
            Debug.DrawRay( (Vector2) transform.position + new Vector2(1f , 2.5f), Vector2.right * 2, debugCollisionColor);

            Debug.DrawRay( (Vector2) transform.position + new Vector2(1f , 1.7f), Vector2.right * 3, debugCollisionColor);
			Debug.DrawRay( (Vector2) transform.position + new Vector2(1f , 1f), Vector2.right * 3.2f, debugCollisionColor);
            Debug.DrawRay( (Vector2) transform.position + new Vector2(1f , 0.3f), Vector2.right * 3, debugCollisionColor);

            Debug.DrawRay( (Vector2) transform.position + new Vector2(1f , -0.5f), Vector2.right * 2, debugCollisionColor);
		}
    }
    private void BasicAttack()
    {
        // Debug.Log("we are trigger");
        if(attackAnimationCounter > 0){
            attackAnimationCounter = -1;
        } else {
            attackAnimationCounter = 1;
        }

        if( !playerCollision.onGround ){
            attackAnimationCounter = -1;
        }

        isAttacking = true;

        animationScript.SetFloat("attackCounter",attackAnimationCounter);
        animationScript.SetTrigger("attack");
        

        playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);

        StartCoroutine(BasicAttackWait());
        StartCoroutine(BasicAttackGroundWait());

    }

    IEnumerator BasicAttackGroundWait(){
        while (isAttacking)
        {
            if( playerCollision.onGround){
                movement.canMove = false;
            } else {
                movement.canMove = true;
            }
            
            yield return null;
        }
        movement.canMove = true;
    }

    IEnumerator BasicAttackWait()
    {

        yield return WaitForFrames(2);

        Vector2 pos = transform.position;

        RaycastHit2D[] hits=  Physics2D.CircleCastAll(pos +  new Vector2(2f , 1f) ,3f, Vector2.right, 0.5f, attackAbleLayerValue);

        // Debug.Log("Triggering/"+hits.Length);
        foreach (RaycastHit2D hit in hits) {

            // if(hit.collider.gameObject.tag == "Enemies"){
                // Debug.Log("----------------Hit Enemies/"+hit.collider.name);
                // hit.collider.gameObject.GetComponent<Interactable>
                // gameObject.SendMessage("ApplyDamage", 5.0);
                hit.collider.gameObject.SendMessage("onHit",playerStat.baseAttackDamage);
            // }
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
