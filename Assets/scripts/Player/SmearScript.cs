using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmearScript : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement move;
    private PlayerAttack attack;

    private bool counter = false;

    public float defaultX;
    public float defaultY;
    public float scaleY;

    void Start()
    {
        anim = GetComponent<Animator>();
        attack = GetComponentInParent<PlayerAttack>();
        move = GetComponentInParent<PlayerMovement>();
    }
    void Update()
    {
        // float x = Input.GetAxis("Horizontal");
        // float y = Input.GetAxis("Vertical");
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        // Debug.Log(y);
        // if (y > 0.5f || y < -0.5f)
        // {
        //     // transform.localPosition = new Vector2(0, y > 0 ? scaleY : -scaleY);
        // }
        // else
        // {
        //     if (move.isFacingRight)
        //     {
        //         // anim.SetFloat("isRightFloat", 1);
        //         transform.localPosition = new Vector2(defaultX, defaultY);
        //     }
        //     else if (!move.isFacingRight)
        //     {
        //         // anim.SetFloat("isRightFloat", -1);
        //         transform.localPosition = new Vector2(-defaultX, defaultY);
        //     }

        // }



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
            }
        }

        if (!attack.isAttacking)
        {

            if ( (y > 0.5f || y < -0.5f ) )
            {
                transform.localPosition = new Vector2(0, y > 0 ? scaleY : -scaleY);
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
            anim.SetFloat("yAxis", y);
            anim.SetFloat("attackCounter", attack.attackAnimationCounter);
        }




    }

}
