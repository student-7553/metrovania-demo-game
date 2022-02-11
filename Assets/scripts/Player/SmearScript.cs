using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmearScript : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement move;
    private PlayerAttack attack;
    private PlayerInput input;
    private PlayerCollision playerCollision;

    private bool counter = false;

    public float defaultX;
    public float defaultY;
    public float downScaleY;
    public float upScaleY;

    void Start()
    {
        anim = GetComponent<Animator>();
        attack = GetComponentInParent<PlayerAttack>();
        move = GetComponentInParent<PlayerMovement>();
        input = GetComponentInParent<PlayerInput>();
        playerCollision = GetComponentInParent<PlayerCollision>();
    }
    void Update()
    {

        float x = input.horizontal;
        float y = input.vertical;

        if (attack.isBasicAttacking)
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
            }
        }

        if (!attack.isBasicAttacking)
        {

            if ((y > 0.5f || (y < -0.5f && !playerCollision.onGround)))
            {
                transform.localPosition = new Vector2(0, y > 0 ? upScaleY : downScaleY);
                if (move.isFacingRight)
                {
                    anim.SetFloat("isRightFloat", 1);
                }
                else if (!move.isFacingRight)
                {
                    anim.SetFloat("isRightFloat", -1);
                }
            }
            else
            {
                if (move.isFacingRight)
                {
                    transform.localPosition = new Vector2(defaultX, defaultY);
                    anim.SetFloat("isRightFloat", 1);
                }
                else if (!move.isFacingRight)
                {
                    transform.localPosition = new Vector2(-defaultX, defaultY);
                    anim.SetFloat("isRightFloat", -1);
                }

            }

            anim.SetFloat("xAxis", x);

            if (y < -0.5f && playerCollision.onGround)
            {

                anim.SetFloat("yAxis", 0);

            }
            else
            {

                anim.SetFloat("yAxis", y);

            }


            anim.SetFloat("attackCounter", attack.attackAnimationCounter);
        }




    }

}
