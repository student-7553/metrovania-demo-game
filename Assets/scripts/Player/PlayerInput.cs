using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;

[DefaultExecutionOrder(-100)]


public class PlayerInput : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float horizontalSoft;
    public float verticalSoft;
    public bool jumpHeld;
    public bool jumpPressed;
    public bool simJumpPressed;
    public bool crouchHeld;
    public bool crouchPressed;
    public bool grabHeld;
    public bool grabPressed;
    public bool dashPressed;
    public bool attackPressed;
    public bool interactPressed;
    public bool rangedPressed;
    public bool focusHeld;


    public bool m_logInput;
    private bool readyToClear;

    public float jumpPressedAllowanceTime;
    private float jumpPressedRemainingAllowedTime = 0f;
    public float attackPressedAllowanceTime = 0.3f;

    private float attackPressedRemainingAllowedTime = 0f;




    void Update()
    {

        ClearInput();
        ProcessInputs();

        if (m_logInput)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    Debug.Log("KeyCode down: " + kcode);
            }
        }

    }

    void FixedUpdate()
    {
        readyToClear = true;
    }

    void ClearInput()
    {
        if (!readyToClear)
        {
            return;
        }

        horizontal = 0f;

        vertical = 0f;

        horizontalSoft = 0f;

        verticalSoft = 0f;


        if (jumpPressedRemainingAllowedTime > jumpPressedAllowanceTime)
        {

            jumpPressed = false;
            jumpPressedRemainingAllowedTime = 0f;
        }

        if (jumpPressedRemainingAllowedTime != 0)
        {

            jumpPressedRemainingAllowedTime = jumpPressedRemainingAllowedTime + Time.deltaTime;
        }

        if (jumpPressed && jumpPressedRemainingAllowedTime == 0f)
        {

            jumpPressedRemainingAllowedTime = Time.deltaTime;

        }

        jumpHeld = false;
        crouchPressed = false;
        crouchHeld = false;
        grabPressed = false;
        grabHeld = false;
        readyToClear = false;
        dashPressed = false;
        focusHeld = false;
        // focusPressed = false;

        if (attackPressedRemainingAllowedTime > attackPressedAllowanceTime)
        {

            attackPressed = false;
            attackPressedRemainingAllowedTime = 0f;
        }

        if (attackPressedRemainingAllowedTime != 0f)
        {
            attackPressedRemainingAllowedTime = attackPressedRemainingAllowedTime + Time.deltaTime;
        }


        if (Input.GetButtonDown("Attack_Basic") && attackPressed)
        {
            attackPressedRemainingAllowedTime = Time.deltaTime;

        }

        if (attackPressed && attackPressedRemainingAllowedTime == 0f)
        {
            attackPressedRemainingAllowedTime = Time.deltaTime;
        }

        interactPressed = false;

        rangedPressed = false;
    }

    void ProcessInputs()
    {

        horizontal = Input.GetAxis("Horizontal");

        vertical = Input.GetAxis("Vertical");

        horizontal = Input.GetAxis("HorizontalSoft");

        vertical = Input.GetAxis("VerticalSoft");

        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");

        jumpHeld = jumpHeld || Input.GetButton("Jump");

        crouchPressed = crouchPressed || Input.GetButtonDown("Crouch");

        crouchHeld = crouchHeld || Input.GetButton("Crouch");

        grabPressed = grabPressed || Input.GetButtonDown("Grab"); ;

        grabHeld = grabHeld || Input.GetButton("Grab");


        dashPressed = dashPressed || Input.GetButtonDown("Dash");

        attackPressed = attackPressed || Input.GetButtonDown("Attack_Basic");

        interactPressed = interactPressed || Input.GetButtonDown("Interact");

        rangedPressed = rangedPressed || Input.GetButtonDown("Ranged");

        focusHeld = focusHeld || (Input.GetAxis("Focus") == 1 ? true : false);


    }

}
