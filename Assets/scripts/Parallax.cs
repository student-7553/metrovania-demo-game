using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos, startCameraPos;
    public GameObject cam;
    public float parallaxEffect;
    public bool isInfinite;

    // Start is called before the first frame update
    void Start()
    {
        
        startpos = transform.position.x;
        
        startCameraPos = cam.transform.position.x;
        
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = ((cam.transform.position.x - startCameraPos) * parallaxEffect);
        float temp = ((cam.transform.position.x - startCameraPos) * (1 - parallaxEffect));
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (isInfinite)
        {


            if (temp > (startpos - startCameraPos) + length)
            {
                startpos = startpos + length;
            }
            else if (temp < (startpos - startCameraPos) - length)
            {
                startpos = startpos - length;
            }
        }




    }
}
