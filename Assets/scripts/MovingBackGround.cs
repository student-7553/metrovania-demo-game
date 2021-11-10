using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackGround : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public float loopXLocalPosition;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(transform.localPosition);
        
        

        if(transform.localPosition.x >= loopXLocalPosition){
            transform.localPosition = new Vector2(-loopXLocalPosition + (transform.localPosition.x - loopXLocalPosition), transform.localPosition.y );
        } else {
            Vector2 newDir = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
            transform.position = newDir;
        }
    }
}
