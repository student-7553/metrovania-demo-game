using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCloneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement playerMovement;
    private bool locked;

    void Start()
    {
        locked = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if(playerMovement.isDashing && !locked){
            locked = true;
            StartCoroutine(DashCloneStart());
        }

        if(!playerMovement.isDashing){
            locked = false;
        }

        if(!locked){
            transform.position = playerMovement.gameObject.transform.position;
        }


    }

    IEnumerator DashCloneStart()
    {
        // yield return WaitForFrames(2);

        yield return new WaitForSeconds(.1f);
        // playerRigidBody.drag = 3;

        // yield return new WaitForSeconds(.1f);
        // playerRigidBody.drag = 9;

        // yield return new WaitForSeconds(.05f);
        // playerRigidBody.drag = 27;


        
    }

    IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
