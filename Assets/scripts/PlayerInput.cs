using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;

[DefaultExecutionOrder(-100)]


public class PlayerInput : MonoBehaviour
{
     public float horizontal;
     public bool jumpHeld;
     public bool jumpPressed;
     public bool crouchHeld;
     public bool crouchPressed;
     public bool grabHeld;
     public bool grabPressed;
     
     public bool dashPressed;

    bool readyToClear;
    // Update is called once per frame
    void Update()
    {
        
        ClearInput();
        ProcessInputs();
        
        // horizontal = Mathf.Clamp(horizontal, -1f, 1f);

    }

    void FixedUpdate(){
        readyToClear = true;
    }

    void ClearInput(){
        if(!readyToClear){
            return;
        }

        horizontal = 0f;
        
        jumpPressed = false;
        jumpHeld = false;
        crouchPressed = false;
        crouchHeld = false;
        grabPressed = false;
        grabHeld = false;
        readyToClear = false;
        dashPressed= false;
    }

    void ProcessInputs()
	{

		horizontal		= Input.GetAxis("Horizontal");

		jumpPressed		= jumpPressed || Input.GetButtonDown("Jump");
    
		jumpHeld		= jumpHeld || Input.GetButton("Jump");

		crouchPressed	= crouchPressed || Input.GetButtonDown("Crouch");

		crouchHeld		= crouchHeld || Input.GetButton("Crouch");

        grabPressed =  grabPressed || Input.GetButtonDown("Grab");;

        grabHeld = grabHeld || Input.GetButton("Grab");

        dashPressed = dashPressed || Input.GetButton("Fire1");
	}

}
