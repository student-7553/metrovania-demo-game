using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCloneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement playerMovement;
    
    private bool locked;

    public class singleChild {
        public GameObject childObject; 
        public bool isLocked;
        public Vector2 lockedVector2;
    }   




    

    private List<singleChild> childObjects; 

    void Start()
    {
        locked = false;
        childObjects = new List<singleChild>();
        foreach (Transform child in transform)
        {
            singleChild data = new singleChild();
            data.childObject = child.gameObject;
            childObjects.Add(data);
        }

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

        // if(!locked){
        transform.position = playerMovement.gameObject.transform.position;
        // }
        
        foreach (singleChild singleChildObject in childObjects) {
            if(singleChildObject.isLocked){
                singleChildObject.childObject.transform.position = singleChildObject.lockedVector2;
            }
        }

        // Debug.Log(childObjects[0].childObject.name);
        // 


    }

    IEnumerator DashCloneStart()
    {
        yield return new WaitForSeconds(.01f);
        Vector2 lockedVector2 = new Vector2( transform.position.x, transform.position.y) ;
        childObjects[0].lockedVector2= lockedVector2;
        childObjects[0].childObject.SetActive(true);
        childObjects[0].isLocked = true;
        StartCoroutine(DashCloneStop(childObjects[0]));

        yield return new WaitForSeconds(.07f);
        lockedVector2 = new Vector2( transform.position.x, transform.position.y) ;
        childObjects[1].lockedVector2= lockedVector2;
        childObjects[1].childObject.SetActive(true);
        childObjects[1].isLocked = true;
        StartCoroutine(DashCloneStop(childObjects[1]));

        yield return new WaitForSeconds(.07f);

        lockedVector2 = new Vector2( transform.position.x, transform.position.y) ;
        childObjects[2].lockedVector2= lockedVector2;
        childObjects[2].isLocked = true;
        childObjects[2].childObject.SetActive(true);
        StartCoroutine(DashCloneStop(childObjects[2]));
        // yield return new WaitForSeconds(.1f);

        // foreach (singleChild singleChildObject in childObjects) {
        //     Debug.Log(singleChildObject.lockedVector2);
        //     singleChildObject.isLocked = false;
        //     singleChildObject.childObject.SetActive(false);
        //     singleChildObject.childObject.transform.position = transform.position;
        // }        
    }
    
    IEnumerator DashCloneStop(singleChild childObjects){
        yield return new WaitForSeconds(.13f);
        childObjects.isLocked = false;
        childObjects.childObject.SetActive(false);
        childObjects.childObject.transform.position = transform.position;
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
