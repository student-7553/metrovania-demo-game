using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackGround : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Vector2 newDir = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        transform.position = newDir;
    }
}
