using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmearScript : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement move;
    private PlayerAttack attack;

    private bool counter = false;
    private string direction = "right";

    public float defaultX;
    public float defaultY;

    void Start()
    {
        anim = GetComponent<Animator>();
        attack = GetComponentInParent<PlayerAttack>();
        move = GetComponentInParent<PlayerMovement>();
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        anim.SetFloat("xAxis", x);
        anim.SetFloat("yAxis", y);

        if (y > 0.5f || y < -0.5f)
        {

            transform.localPosition = new Vector2(0 , defaultY);

            // Vertical Attack

            if (move.isFacingRight )
            {
                // direction = "right";
                anim.SetFloat("isRightFloat", 1);
                // transform.localPosition = new Vector2(transform.localPosition.x > 0 ? transform.localPosition.x : -transform.localPosition.x, transform.localPosition.y);
            }
            else if (!move.isFacingRight )
            {
                // direction = "left";
                anim.SetFloat("isRightFloat", -1);
                // transform.localPosition = new Vector2(transform.localPosition.x > 0 ? -transform.localPosition.x : transform.localPosition.x, transform.localPosition.y);
            }


        }
        else
        {
            // if(){

            // }
            // transform.localPosition = new Vector2( defaultX , defaultY);

            // Horizontal attack
            // if (move.isFacingRight && direction == "left")
            // {
            //     direction = "right";
            //     anim.SetFloat("isRightFloat", 1);
            //     transform.localPosition = new Vector2( defaultX , defaultY);
            // }
            // else if (!move.isFacingRight && direction == "right")
            // {
            //     direction = "left";
            //     anim.SetFloat("isRightFloat", -1);
            //     transform.localPosition = new Vector2( -defaultX  , defaultY);
            // }
            if ( move.isFacingRight )
            {
                direction = "right";
                anim.SetFloat("isRightFloat", 1);
                transform.localPosition = new Vector2( defaultX , defaultY);
            }
            else if (!move.isFacingRight)
            {
                direction = "left";
                anim.SetFloat("isRightFloat", -1);
                transform.localPosition = new Vector2( -defaultX  , defaultY);
            }

        }

        anim.SetFloat("attackCounter", attack.attackAnimationCounter);

        if (attack.isAttacking)
        {
            if (!counter)
            {
                counter = true;
                anim.SetTrigger("triggered");
            }
        }
        else
        {
            if (counter)
            {
                counter = false;
                // anim.SetTrigger("triggered");
            }
        }


    }

}
