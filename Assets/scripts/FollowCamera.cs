using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera mainCamera;

    public GameObject referenceCamera;

    void Start()
    {
        mainCamera = referenceCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = new Vector2(transform.position.x , 0 );

        if(mainCamera.transform.position.y > 7f){
            dir.y = 15f;
        }
        

        transform.position = dir;
    }
}
