using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ShakeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float deltaXMin;
    public float deltaXMax;
    public float deltaYMin;
    public float deltaYMax;
    public float delayMax;
    private Vector2 startPos;
    private bool looping;

    private bool falling;
    private float ramp;
    private float eclipsedTime;
    private SpriteRenderer currentRenderer;

    void Start()
    {
        startPos = transform.localPosition;

    }
    void OnEnable() {
        currentRenderer = GetComponent<SpriteRenderer>();
        if(looping){
            transform.localPosition = startPos;

        }
        ramp = 0.05f;
        looping = false;
        falling = false;
        Color newColor = new Color(1, 1, 1, 1);
        currentRenderer.color = newColor;
        eclipsedTime = 0;
    }

    void Update()
    {   
 
        if(looping){
            if(ramp < 1f){
                ramp = ramp + (0.05f * Time.deltaTime);
            }

            if(eclipsedTime > 1.5f && !falling){
                falling = true;
                Debug.Log("are we here?");
                DOVirtual.Float(1f, 0f, 0.5f, FadeTo);
                
            }

            eclipsedTime = eclipsedTime + Time.deltaTime;
        }
    }

    void FadeTo(float x)
    {

        if (!looping)
        {
            return;
        }
        Color newColor = new Color(1, 1, 1, x);
        currentRenderer.color = newColor;


    }

    void signalShake()
    {
        StartCoroutine(shaking());
    }



    IEnumerator shaking()
    {
    
        looping = true;
        while (looping && !falling)
        {
            float rangeX = Random.Range(startPos.x - ((deltaXMin) * ramp), startPos.x + (deltaXMax * ramp));
            float rangeY = Random.Range(startPos.y - (deltaYMin * ramp), startPos.y + (deltaYMax * ramp));

            float delay = Random.Range(0f, delayMax);

            Vector2 newPosition = new Vector2(rangeX , rangeY );

            transform.localPosition = newPosition;
            
            yield return new WaitForSeconds(delay);
        }
        
        while(falling){

            float rangeX = Random.Range(startPos.x - ((deltaXMin) * ramp), startPos.x + (deltaXMax * ramp));

            Vector2 newPosition = new Vector2(rangeX , transform.localPosition.y - 0.2f );

            transform.localPosition = newPosition;
            
            yield return new WaitForSeconds(0.1f);
        }

    }

}
