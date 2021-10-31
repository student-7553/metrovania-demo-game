using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParallaxVertical : MonoBehaviour
{

    private float startpos, startCameraPos;
    public GameObject cam;
    public float parallaxEffect;
    

    void Start()
    {
        startpos = (float)Mathf.Round(transform.position.y * 1000f) / 1000f;
        
        startCameraPos = cam.transform.position.y;
    }

    void Update()
    {
    
        float dist = ((cam.transform.position.y - startCameraPos) * parallaxEffect);
        
        float newHeight = startpos + dist;
        newHeight = (float)Mathf.Round(newHeight * 1000f) / 1000f;
 
        float tempHeight =  newHeight - (float)Mathf.Round(cam.transform.position.y * 1000f) / 1000f;
        float newFixedHeight = ((int)(tempHeight / 0.125f)) * 0.125f;

        transform.localPosition = new Vector2(transform.localPosition.x , newFixedHeight );
  
        
        
        
    
    }
}
