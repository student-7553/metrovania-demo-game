using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAfterEffect : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement move;
    private PlayerInput input;

    private bool counter = false;

    public float defaultX;
    public float defaultY;
    public float downScaleY;
    public float upScaleY;

    private Vector2 lockedPosition;
    void Start()
    {
        anim = GetComponent<Animator>();
        move = GetComponentInParent<PlayerMovement>();
        input = GetComponentInParent<PlayerInput>();

        // anim.enabled = false;
        anim.SetFloat("isRightFloat", 1);
    }

    // Update is called once per frame
    void Update()
    {
        float x = input.horizontal;
        float y = input.vertical;

        if (move.isDashing)
        {
            if (!counter)
            {
                // Debug.Log("triggering?");
                counter = true;
                anim.SetTrigger("triggered");
                // anim.enabled = true;
                lockedPosition = transform.position;
            }
        }
        else
        {
            if (counter)
            {
                counter = false;
                // anim.enabled = false;
            }
        }

        if (counter)
        {
            transform.position = lockedPosition;
        }


        if (!move.isDashing)
        {

            if ((y > 0.5f || y < -0.5f))
            {

                if (x > 0.5f && y > 0.5f)
                {
                    // upRight
                    transform.localPosition = new Vector2(0, defaultY);
                    transform.eulerAngles = new Vector3(0f, 0f, 45f);
                }
                else if (x < 0.5f && y > 0.5f)
                {
                    // upLeft
                    transform.localPosition = new Vector2(0, defaultY);
                    transform.eulerAngles = new Vector3(0f, 0f, 135f);
                }
                else if (x < 0.5f && y < 0.5f)
                {
                    // downLeft
                    transform.localPosition = new Vector2(0, defaultY);
                    transform.eulerAngles = new Vector3(0f, 0f, 225f);
                }
                else if (x > 0.5f && y < 0.5f)
                {
                    // downRight
                    transform.localPosition = new Vector2(0, defaultY);
                    transform.eulerAngles = new Vector3(0f, 0f, 315f);
                }


            }
            else
            {
                if (move.isFacingRight)
                {
                    transform.localPosition = new Vector2(-defaultX, defaultY);
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    // anim.SetFloat("isRightFloat", 1);
                }
                else if (!move.isFacingRight)
                {
                    transform.localPosition = new Vector2(defaultX, defaultY);
                    transform.eulerAngles = new Vector3(180f, 0f, 180f);
                    // anim.SetFloat("isRightFloat", -1);
                }

            }

            // anim.SetFloat("xAxis", x);
            // anim.SetFloat("yAxis", y);
            // anim.SetFloat("attackCounter", attack.attackAnimationCounter);
        }


    }
}
