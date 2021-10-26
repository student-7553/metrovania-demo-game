using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxVertical : MonoBehaviour
{
    // Start is called before the first frame update

    private float length, startpos, startCameraPos;
    public GameObject cam;
    public float parallaxEffect;
    // public bool isInfinite;

    void Start()
    {
        startpos = transform.position.y;
        
        startCameraPos = cam.transform.position.y;
        
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = ((cam.transform.position.y - startCameraPos) * parallaxEffect);
        float temp = ((cam.transform.position.y - startCameraPos) * (1 - parallaxEffect));
        transform.position = new Vector3(transform.position.x , startpos + dist, transform.position.z);
        

        // if (isInfinite)
        // {


        //     if (temp > (startpos - startCameraPos) + length)
        //     {
        //         startpos = startpos + length;
        //     }
        //     else if (temp < (startpos - startCameraPos) - length)
        //     {
        //         startpos = startpos - length;
        //     }
        // }




    }
}
